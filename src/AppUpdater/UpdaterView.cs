using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Common.Logging;

namespace AppUpdater
{
    public partial class UpdaterView : Form
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(UpdaterView));

        #region  Properties and Field

        private String ApplicationName { get; set; }

        private String title = "A new version of {0} is available.Do you want to download it now?";
        
        private String NewVersion
        {
            get { return lblUpdateVersion.Text; }
            set { lblUpdateVersion.Text = value; }
        }

        private String InstalledVersion
        {
            get { return lblInstalledVersion.Text; }
            set { lblInstalledVersion.Text = value; }
        }

        internal Updater updater;

        private BackgroundWorker downloadWorker;

        private int PacketSize = 1024 * 5;      // 5KB at a single chunk

        private int TotalProgress;

        private bool IsCancel = false;

        public bool IsDownloading = false;

        private bool IsStartDownload = false;
        #endregion

        #region Constructor

        private UpdaterView()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            downloadui.Visible = false;
            downloadui.ShowFileName = true;
            downloadui.ShowRemainTime = false;

            downloadWorker = new BackgroundWorker
                                 {
                                     WorkerReportsProgress = true,
                                     WorkerSupportsCancellation = true
                                 };

            downloadWorker.DoWork += DownloadWorkerDoWork;
            downloadWorker.ProgressChanged += DownloadWorkerProgressChanged;
            downloadWorker.RunWorkerCompleted += DownloadWorkerRunWorkerCompleted;
        }

        public UpdaterView(String url, String username, String password, bool start)
            : this()
        {
            updater = new Updater(url, username, password);

            IsStartDownload = start;
            NewVersion = updater.GetNewVersion();
            InstalledVersion = updater.InstalledVersion;
        }

        public UpdaterView(String appname, String installv, String url, String username, String password, bool start)
            : this()
        {
            updater = new Updater(url, username, password) { InstalledVersion = installv };

            IsStartDownload = start;
            ApplicationName = appname;
            InstalledVersion = updater.InstalledVersion;
            lblTitle.Text = String.Format(title, ApplicationName);
            NewVersion = updater.GetNewVersion();
        }

        #endregion

        #region Methods

        void DownloadWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            StartDownload();
        }

        void DownloadWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            downloadui.SetPercentageCompleted(e.ProgressPercentage);
        }

        void DownloadWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsDownloading = false;

            if (e.Cancelled)
                return;

            if (IsCancel)
            {
                downloadui.SetPercentageCompleted(0);
                MessageBox.Show(this, "Update Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Info("Update Failed");
            }
            else
            {
                downloadui.SetPercentageCompleted(100);

                String runfile = updater.TempPath + "\\" + updater.VersionObj.StartupFile;
                // run the main file
                bool runstatus = updater.RunStartupFile(runfile);

                Log.Info("Update " + ((runstatus) ? "Complete" : "Failed"));
            }

            Application.Exit();
        }

        private void StartDownload()
        {
            TotalProgress = 0;
            IsCancel = false;

            try
            {
                bool initStatus = updater.ReadDownloadItemInfo();

                if (!initStatus || updater.Files == null)
                    throw new Exception("Files list is null");

                // TempPath + App Name + Added Version Folder 
                updater.TempPath = Path.Combine(Path.Combine(updater.TempPath, this.ApplicationName),
                                    updater.NewVersion);

                int successfullDownload = 0;
                int count = 0;

                while (count < updater.Files.Count && !downloadWorker.CancellationPending)
                {
                    bool success = updater.IsFtp ? DownloadSingleFileFTP(updater.Files[count], updater.TempPath)
                        : DownloadSingleFileHttp(updater.Files[count], updater.TempPath);

                    if (downloadWorker.CancellationPending)
                    {
                        // cancel the download and all event 
                        IsCancel = true;
                        break;
                    }

                    count += 1;
                    successfullDownload += (success) ? 1 : 0;
                }

                if (!IsCancel && (count != successfullDownload))
                    throw new Exception("Download Failed, " + (count - successfullDownload) + " Files");
            }
            catch (Exception ex)
            {
                IsCancel = true;
                Log.Error(ex.Message);
                Log.Debug(ex.StackTrace);
            }
        }
        
        private bool DownloadSingleFileHttp(ItemInfo file, String dstDir)
        {
            bool isSuccess = true;
            Byte[] readBytes = new Byte[PacketSize];

            // create destination directory if not exits
            String dir = Path.GetDirectoryName(Path.Combine(dstDir, file.FullPath));
            if (!Directory.Exists(dir) || !string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            HttpWebRequest webReq;
            HttpWebResponse webResp = null;

            try
            {
                webReq = (HttpWebRequest)WebRequest.Create(file.DownloadUrl);
                webReq.Credentials = updater.GetCredential();
                webReq.KeepAlive = false;
                webReq.Method = updater.IsFtp ? WebRequestMethods.Ftp.DownloadFile : WebRequestMethods.Http.Get;
                
                webReq.Credentials = updater.GetCredential();
                webResp = (HttpWebResponse)webReq.GetResponse();

                downloadui.SetDownloadingFileName(file.FileName);

                using (FileStream writer = new FileStream(Path.Combine(dir, file.FileName), FileMode.Create))
                {
                    int currentFileProgress = 0;

                    while (currentFileProgress < file.FileSize && !downloadWorker.CancellationPending)
                    {
                        // while (this.IsPaused) { System.Threading.Thread.Sleep(50); }

                        var responseStream = webResp.GetResponseStream();

                        if (responseStream != null)
                        {
                            Int32 currentPacketSize = responseStream
                                .Read(readBytes, 0, PacketSize);

                            currentFileProgress += currentPacketSize;
                            TotalProgress += currentPacketSize;
                            double percent = (TotalProgress / (updater.TotalFileSize * 1.0)) * 100;

                            downloadWorker.ReportProgress((int)percent);

                            writer.Write(readBytes, 0, currentPacketSize);
                        }
                    }

                    writer.Flush();
                    writer.Close();
                }

                webResp.Close();
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Log.Error(ex.Message);
                Log.Debug(ex.StackTrace);
            }

            return isSuccess;
        }

        private bool DownloadSingleFileFTP(ItemInfo file, String dstDir)
        {
            bool isSuccess = true;
            Byte[] readBytes = new Byte[PacketSize];

            // create destination directory if not exits
            String dir = Path.GetDirectoryName(Path.Combine(dstDir, file.FullPath));
            if (!Directory.Exists(dir) || !string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            FtpWebRequest webReq;
            FtpWebResponse webResp = null;

            try
            {
                webReq = (FtpWebRequest)WebRequest.Create(file.DownloadUrl);
                webReq.Credentials = updater.GetCredential();
                webReq.KeepAlive = false;
                webReq.UsePassive = true;
                webReq.Method = updater.IsFtp ? WebRequestMethods.Ftp.DownloadFile : WebRequestMethods.Http.Get;
                
                webReq.Credentials = updater.GetCredential();
                webResp = (FtpWebResponse)webReq.GetResponse();

                downloadui.SetDownloadingFileName(file.FileName);

                using (FileStream writer = new FileStream(Path.Combine(dir, file.FileName), FileMode.Create))
                {
                    int currentFileProgress = 0;

                    while (currentFileProgress < file.FileSize && !downloadWorker.CancellationPending)
                    {
                        // while (this.IsPaused) { System.Threading.Thread.Sleep(50); }

                        var responseStream = webResp.GetResponseStream();

                        if (responseStream != null)
                        {
                            Int32 currentPacketSize = responseStream
                                .Read(readBytes, 0, PacketSize);

                            currentFileProgress += currentPacketSize;
                            TotalProgress += currentPacketSize;
                            double percent = (TotalProgress / (updater.TotalFileSize * 1.0)) * 100;

                            downloadWorker.ReportProgress((int)percent);

                            writer.Write(readBytes, 0, currentPacketSize);
                        }
                    }

                    writer.Flush();
                    writer.Close();
                }

                webResp.Close();
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Log.Error(ex.Message);
                Log.Debug(ex.StackTrace);
            }

            return isSuccess;
        }


        private void UpdaterViewLoad(object sender, EventArgs e)
        {
            if (IsStartDownload)
                BtnDownloadClick(this, e);
        }

        public void BtnDownloadClick(object sender, EventArgs e)
        {
            if (IsDownloading)
            {
                DialogResult dr = MessageBox.Show("Are you sure to cancel the downloading ?", "Confim", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                    return;

                BtnDownload.Text = "Download";
                BtnSkip.Visible = true;

                Log.Info("Canceling Download.");
                downloadWorker.CancelAsync();
            }
            else
            {
                Log.Info("Starting Download.");

                downloadui.Show();
                downloadWorker.RunWorkerAsync();

                BtnDownload.Text = "Cancel";
                BtnSkip.Visible = false;
            }

            IsDownloading = !IsDownloading;
        }

        private void BtnSkipClick(object sender, EventArgs e)
        {
            if (IsDownloading)
            {
                //return;
            }

            this.Close();
        }

        private void UpdaterViewFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDownloading)
            {
                e.Cancel = true;
            }
        }

        #endregion
    }
}

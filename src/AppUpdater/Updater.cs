using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Common.Logging;

namespace AppUpdater
{
    public class Updater : IDisposable
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(Updater));

        #region Properties 
        
        private readonly String checkfile = "Version.xml";
        public String DefaultVersionFileName 
        {
            get { return checkfile; }            
        }

        public Uri Server { get; set; }        
        public String Username { get; set; }
        public String Password { get; set; }
        public String TempPath { get; set; }
        public List<ItemInfo> Files { get; set; }
        public long TotalFileSize { get; set; }

        private String availableVersion;
        public String NewVersion 
        {
            get
            {                
                //if (String.IsNullOrEmpty(this.availableVersion))
                //{
                //    this.availableVersion = this.GetNewVersion();
                //}

                return availableVersion;
            }

            private set { }
        }
                
        public String InstalledVersion { get; set; }

        public Boolean IsUpdateRequired 
        {
            get
            {
                if (String.IsNullOrEmpty(this.NewVersion))
                    return false;

                return !NewVersion.Equals(InstalledVersion);
            }            
        }
        
        private NetworkCredential nCredential = null;
        
        private VersionFile versionFile = null;
        public VersionFile VersionObj
        {
            get
            {
                return  versionFile ?? this.GetVersionFile();
            }
            private set
            {
                versionFile = value;
            }
        }

        public bool IsFtp { get; set; }

        // use any temp name for download folder 
        public String TempFolder = "AppUpdates";
        
        #endregion

        #region Constructor
        private Updater() 
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            TempPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), TempFolder);
        }

        public Updater(Uri Url)
            : this()
        {
            this.Server = Url;
            IsFtp = this.Server.Scheme.StartsWith("FTP");
        }

        public Updater(String Url)
            : this(new Uri(Url))
        { }

        public Updater(Uri Url, String username, String passwd)
            : this(Url)
        {
            this.Username = username;
            this.Password = passwd;                      
        }

        public Updater(String Url, String username, String passwd)
            : this(new Uri(Url), username, passwd)
        { }

        #endregion

        #region Methods
     
        public String GetNewVersion()
        {
            availableVersion = VersionObj.NewVersion;

            return availableVersion;
        }
                             
        private VersionFile GetVersionFile()
        {
            VersionFile vfile = new VersionFile();
            bool status = true;

            Log.Info("Updater : Getting " + DefaultVersionFileName);            

            try
            {                
                WebRequest request = WebRequest.Create(this.Server + this.DefaultVersionFileName);
                request.Credentials = GetCredential();

				// Added the function to support proxy
                request.Proxy = WebProxy.GetDefaultProxy();
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;                
				// End added
                
                using (WebResponse response = request.GetResponse())
                {
                    vfile = vfile.GetVersionFileFromStream(response.GetResponseStream());
                    this.versionFile = vfile;
                }
            }
            catch (WebException ex) 
            {
                status = false;
                
                Log.Error(ex.Message);
                Log.Debug(ex.StackTrace);
            }
            catch(Exception ex) 
            {
                status = false;
                
                Log.Error(ex.Message);
                Log.Debug(ex.StackTrace);
            }

            Log.Info("Updater : Version File load " + ((status) ? "Success" : "Failed"));

            return vfile;
        }

        public void SetCredential(String user, String pass)
        {
            this.Username = user;
            this.Password = pass;

            nCredential = new NetworkCredential(this.Username, this.Password);
        }

        public NetworkCredential GetCredential()
        {
            SetCredential(this.Username, this.Password);

            return nCredential;
        }
                   
        public bool ReadDownloadItemInfo()
        {
            bool status = true;
            TotalFileSize = 0;
            
            try
            {                
                if ( versionFile.Files == null && String.IsNullOrEmpty(versionFile.NewVersion) )
                    this.GetVersionFile();

                this.Files = DetailFileInfo(VersionObj.Files.ToList());
                            
                foreach (ItemInfo t in this.Files)
                    TotalFileSize += t.FileSize;
            }
            catch (Exception ex)
            {
                status = false;
                
                Log.Error(ex.Message);
                Log.Debug(ex.StackTrace);
                //throw new Exception("Download Initializtion Failed.");
            }

            return status;
        }
               
        private List<ItemInfo> DetailFileInfo(IList<ItemInfo> files)
        {
            List<ItemInfo> detailList = new List<ItemInfo>();

            foreach (ItemInfo f in files)
            {
                String fullurl = this.Server + "//" + this.NewVersion + "//" + f.FileName;
                ItemInfo item = GetItemInfoDetail(fullurl, f);

                detailList.Add(item);
            }

            return detailList;
        }
        
        private ItemInfo GetItemInfoDetail(String url, ItemInfo it)
        {
            WebRequest request = WebRequest.Create(url);

            request.Method = (this.Server.Scheme.StartsWith("FTP",StringComparison.InvariantCultureIgnoreCase))
                ? WebRequestMethods.Ftp.GetFileSize : WebRequestMethods.Http.Head;

            request.Credentials = new NetworkCredential(this.Username, this.Password);

            ItemInfo item = new ItemInfo();

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    item.FileSize = response.ContentLength;

                    item.ContentType = response.ContentType;
                }
            }
            catch (WebException ex) { }
            catch (Exception ex) { }

            item.DownloadUrl = request.RequestUri.ToString();
            item.FullPath =  it.FileName;
            item.FileName = Path.GetFileName(it.FileName);
            
            return item;
        }

        public bool RunStartupFile(String runfile)
        {
            bool retVal = true;

            try
            {
                if (!File.Exists(runfile))
                    throw new Exception("Startup File Not Found");

                Directory.SetCurrentDirectory(Path.GetDirectoryName(runfile));
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = Path.GetFileName(runfile);
                    p.StartInfo.CreateNoWindow = false;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    p.Start();
                }                
                //Directory.Delete(Path.GetFullPath(runfile), true);                                             
            }
            catch (Exception ex)
            {
                retVal = false;
                Log.Error("Startup File Run Failed.");
                Log.Debug(ex.Message);
                Log.Debug(ex.StackTrace);
            }

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            return retVal;
        }

        public void Backup()
        {
            throw new NotImplementedException();
        }

        public bool Install()
        {
            throw new NotImplementedException();
            return false;
        }

        public bool Rollback()
        {
            throw new NotImplementedException();
            return false;
        }
                
        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            
        }

        #endregion
    }
}

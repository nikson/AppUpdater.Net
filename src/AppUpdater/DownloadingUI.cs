using System;
using System.Windows.Forms;

namespace AppUpdater
{
    public partial class DownloadingUI : UserControl
    {
        String _filename = "Downloading ../../{0}"; 
        String _completed = "{0}%";
        String _remaintime = "Remains: {0}";

        private Boolean showFileName;
        public Boolean ShowFileName
        {
            get { return showFileName;}
            set
            {
                showFileName = value;
                if (!value) dlFilename.Text = "Downloading";
            }
        }

        public Boolean ShowRemainTime 
        {
            get { return dlRemaintime.Visible; }
            set { dlRemaintime.Visible = value; }
        }

        public String CurrentFile { get; private set; }
        public TimeSpan RemainTime { get; private set; }       

        public Int32 PercentCompleted { get; private set; }

        public DownloadingUI()
        {
            InitializeComponent();

            dlFilename.Text = "Downloading";

            showFileName = true;
            ShowRemainTime = false;
        }
       
        public void SetDownloadingFileName(String arg)
        {
            this.CurrentFile = arg;

            if (this.InvokeRequired)
            {
                MethodInvoker min = delegate
                {
                    if (this.showFileName)
                    {
                        dlFilename.Text = String.Format(_filename, arg);
                    }
                };

                min.Invoke();
                return;
            }

            if (this.showFileName)
            {
                dlFilename.Text = String.Format(_filename, arg);
            }
        }

        int _ticks = 10000000;      // 1 Second = 10000000 Ticks
        int _second = 1;
        int _minute = 60;           // 1 minute = 60 sec
        int _hour = 3600;           // 1 hour = 3600sec 
        int _day = 86400;           // 1 day = 86400sec
        String timeStr = "{0}{1} ";

        public void SetRemainsTime(int tsecond)
        {            
            int day , hour , minute , second;
            int _rem = 0;

            day = Math.DivRem(tsecond, _day, out _rem);
            hour = Math.DivRem(_rem, _hour, out _rem);
            minute = Math.DivRem(_rem, _minute, out _rem);
            second = Math.DivRem(_rem, _second, out _rem);

            String s = ( (day != 0) ? String.Format(timeStr, day, "d") : "") +
                ((hour != 0) ? String.Format(timeStr, hour, "h") : "") +
                ((minute != 0) ? String.Format(timeStr, minute, "m") : "");
                //((second != 0) ? String.Format(timeStr, second, "s") : "");

            s = ((s.Trim().Length == 0) ? String.Format(timeStr, second, "s") : s);                

            TimeSpan t = new TimeSpan(day, hour, minute, second);
            this.RemainTime = t;

            //Console.WriteLine(t.Days.ToString() + " " + t.Hours.ToString() + " " + t.Minutes.ToString() + " " 
            //    + t.Seconds.ToString());
            
            SetRemainsTime(s.Trim());
        }

        private void SetRemainsTime(object o)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker mIn = delegate { SetRemainsTime(o); };
                this.Invoke(mIn);
                return;
            }

            dlRemaintime.Text = String.Format(_remaintime, o.ToString());
        }
              
        public void SetPercentageCompleted(int value)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker min = delegate
                    {
                        _progressbar.Value = value;
                        this.PercentCompleted = value;

                        dlCompleted.Text = String.Format(_completed, value);
                    };

                    min.Invoke();
                    return;
                }

                _progressbar.Value = value;
                this.PercentCompleted = value;
                dlCompleted.Text = String.Format(_completed, value);
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }

        public void SetPercentageCompleted(long curr, long total)
        {
            if (total < curr)
                throw new ArgumentException("Value is greater then Total", "Current Value");

            double p = ((curr * 100.0) / total);
            int value = (int)Math.Round(p);

            SetPercentageCompleted(value);
        }
                      
    }
}

using System;
using System.Windows.Forms;
using Common.Logging;

namespace AppUpdater
{
    static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params  String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                Log.Info("Starting Application Updater");
                
                if (args.Length == 4)
                {
                    Uri server;
                    if (!Uri.TryCreate(args[0], UriKind.Absolute, out server))
                        throw new Exception("Invalid URL.");

                    bool start;
                    if (!Boolean.TryParse(args[3], out start))
                        throw new Exception("Invalid argument");
                    
                    UpdaterView uv = new UpdaterView(server.ToString(), args[1], args[2], start);
                    //new UpdaterView("url", "user", "pass", true);

                    Application.Run(uv);
                }
                else if (args.Length == 6)
                {
                    Uri server;
                    if (!Uri.TryCreate(args[2], UriKind.Absolute, out server))
                        throw new Exception("Invalid URL.");

                    bool start;
                    if (!Boolean.TryParse(args[5], out start))
                        throw new Exception("Invalid argument");

                    UpdaterView uv = new UpdaterView(args[0], args[1], server.ToString(), args[3], args[4],start);
                    //new UpdaterView("Appname", "installv", "url", "user", "pass", true);
                    
                    Application.Run(uv);
                }
                else
                {
                    Log.Error("Updater : Invalid Parameters");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                Log.Debug(e.StackTrace);
                Log.Error("Updater : " + e.Message);
                
                Application.Exit();
            }
            finally
            {
                Log.Info("Exit Application Updater");
            }
        }
    }
}

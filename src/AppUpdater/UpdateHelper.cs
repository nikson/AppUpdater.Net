using System;
using System.Windows.Forms;
using AppUpdater;
using System.IO;
using System.Diagnostics;

public class UpdateHelper
{
    private static Updater u;
    private static String UpdateServer = "ftp://mysite.com/";
    private static String username = "username";
    private static String password = "password";
    private static String AppName = "MyApp";
    private const String updaterExe = "AppUpdater.exe";

    static UpdateHelper()
    {
        try
        {
            u = new Updater(UpdateServer, username, password);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Update Server Url Error. Please check add the valid UpdateServer URL.");
        }
    }

    public static bool CheckUpdate()
    {
#if DEBUG
        // debug mode is Developing mode so do not need to check it in debug mode            
        return false;
#endif

        bool ret = false;
        try
        {
            // Set Install Version  
            u.InstalledVersion = "Current Version";
            var ver = u.GetNewVersion();
            ret = u.IsUpdateRequired;

            if (ret)
                MessageBox.Show("An update is available. version:" + u.NewVersion);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Update Checking: an execption is occured");

            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RunVersionUpdater()
    {

        DialogResult dr = MessageBox.Show(
            "An update of this application is available. Do you want to update now\n\n" +
            "New Verison available: " + u.NewVersion +
            "\n\nApplication will be restarted. Please save all information before continue.",
            "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

        if (dr == DialogResult.No)
            return;
        else
        {
            try
            {

                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                Process p = new Process();
                p.StartInfo.FileName = Path.GetFileName(updaterExe);
                p.StartInfo.CreateNoWindow = false;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                p.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"",
                    AppName, u.InstalledVersion, UpdateServer, username, password, true);
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Application.Exit();
        }
    }
}

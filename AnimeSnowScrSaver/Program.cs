using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace AnimeSnowScrSaver
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //MessageBox.Show(args.Length.ToString());
            if (args.Length > 0)
            {
                //MessageBox.Show(args[0].Trim().ToLower());
                switch (args[0].Trim().ToLower())
                {
                    case "/c":
                        Application.Run(new Setting());
                        break;
                    case "/s":
                        RunAnimeSnow();
                        break;
                    default:
                        return;
                }
            }
        }

        private static void RunAnimeSnow()
        {
            Microsoft.Win32.RegistryKey rootKey = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey runKey = rootKey.OpenSubKey(@"Software\AnimeSnow");
            if (runKey == null || runKey.GetValue("ExePathName") == null)
            {
                MessageBox.Show("没有找到主程序，请先运行一次桌面下雪工具！");
                rootKey.Close();
                return;
            }
            else
            {
                string exePath = runKey.GetValue("ExePathName").ToString();
                rootKey.Close();
                if (File.Exists(exePath))
                {
                    Process process = new Process();
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
                    process.StartInfo.FileName = exePath;
                    //MessageBox.Show(Path.GetDirectoryName(exePath));
                    process.StartInfo.Arguments = "/scr";
                    process.Start();
                    if (process != null)
                    {
                        process.WaitForExit();
                    }
                }
            }

            //MessageBox.Show("ok");
        }
    }
}

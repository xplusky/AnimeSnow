using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace AnimeSnow
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            //MessageBox.Show(e.Args.Length.ToString());
            if (e.Args.Length > 0 && e.Args[0].Trim().ToLower() == "/scr")
            {
                IsScrSaver = true;
            }
        }

        public static bool IsScrSaver = false;
    }
}

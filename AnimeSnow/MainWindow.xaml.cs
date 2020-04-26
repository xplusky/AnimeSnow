using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using forms = System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace AnimeSnow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 全局变量

        //鼠标穿透相关
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int WS_EX_TOOLWINDOW = 0x00000080;
        const int GWL_EXSTYLE = -20;
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        //钉在桌面(最下层)
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        //系统托盘图标
        forms.NotifyIcon notifyIcon = null;

        //Storyboard相关
        Storyboard SBExit;
        public Storyboard SBMiddiumSnow;
        public Storyboard SBSmallSnow;

        #endregion
        //入口
        public MainWindow()
        {
            InitializeComponent();
            AppReg();
            //Storyboard Init
            SBExit = (Storyboard)Resources["SBExit"];
            SBMiddiumSnow = (Storyboard)Resources["SBMiddiumSnow"];
            SBSmallSnow = (Storyboard)Resources["SBSmallSnow"];
            this.SourceInitialized += new EventHandler(MainWindow_SourceInitialized);
            SBMiddiumSnow.Begin();
            SBSmallSnow.Begin();
        }

        private void AppReg()
        {
            try
            {
                Microsoft.Win32.RegistryKey rootKey = Microsoft.Win32.Registry.CurrentUser;
                Microsoft.Win32.RegistryKey runKey = rootKey.CreateSubKey(@"Software\AnimeSnow");
                runKey.SetValue("ExePathName", Process.GetCurrentProcess().MainModule.FileName);
                runKey.Close();
            }
            catch { }
            
        }

        //初始化完毕
        void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            InitTray();
            this.notifyIcon.MouseClick += new forms.MouseEventHandler(notifyIcon_MouseClick);
            this.notifyIcon.MouseDoubleClick += new forms.MouseEventHandler(notifyIcon_MouseDoubleClick);
            if (App.IsScrSaver)
            {
                this.MouseMove += MainWindow_MouseMove;
            }
            else
                mousePierce();
            SettingRead();
            
        }

        void MainWindow_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.MouseMove -= MainWindow_MouseMove;
            Exit();
        }

        void notifyIcon_MouseDoubleClick(object sender, forms.MouseEventArgs e)
        {
            menuSetting_Click(null, null);
        }

        void notifyIcon_MouseClick(object sender, forms.MouseEventArgs e)
        {
            this.Activate();
        }

        //读取设置
        void SettingRead()
        {
            Setting setting = new Setting();
            GridMiddium.Opacity = setting.Opacity1;
            GridSmall.Opacity = setting.Opacity2;
            SBMiddiumSnow.SetSpeedRatio(setting.Speed);
            SBSmallSnow.SetSpeedRatio(setting.Speed);
            SetWindowPosition(setting.WindowPos);
        }

        //初始化系统托盘图标
        void InitTray()
        {
            notifyIcon = new forms.NotifyIcon();
            notifyIcon.Text = "桂叶雪花飘落动态桌面";
            
            notifyIcon.Icon = new System.Drawing.Icon(Directory.GetParent(Process.GetCurrentProcess().MainModule.FileName).ToString() + @"\Res\Icon\SnowDownIcon.ico");
            notifyIcon.Visible = true;

            forms.MenuItem menuSetting = new forms.MenuItem("设置", new EventHandler(menuSetting_Click));
            forms.MenuItem menuAbout = new forms.MenuItem("关于", new EventHandler(menuAbout_Click));
            forms.MenuItem menuExit = new forms.MenuItem("退出", new EventHandler(menuExit_Click));
            forms.MenuItem[] menus = new forms.MenuItem[] { menuSetting,menuAbout,menuExit };
            notifyIcon.ContextMenu = new forms.ContextMenu(menus);
            
        }

        //为了能够穿透窗体
        void mousePierce()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW);
        }

        //设置窗体层次
        public void SetWindowPosition(int index)
        {
            if (index == 0)//置于顶层
            {
                this.Topmost = true;
            }
            if (index == 1)//置于底层
            {
                this.Topmost = false;
                User32 user32 = new User32();
                try
                {
                    SetWindowPos(new WindowInteropHelper(this).Handle, 1, 0, 0, 0, 0, 0x13);
                }
                catch { }
            }
        }

        //响应系统托盘菜单功能
        void menuSetting_Click(object sender, EventArgs e)
        {
            if (!Common.isSettingWindowShow)
            {
                SettingWindow settingWindow = new SettingWindow(this);
                settingWindow.Show();
                Common.isSettingWindowShow = true;
            }
        }
        void menuAbout_Click(object sender,EventArgs e)
        {
            if (!Common.isAboutWindowShow)
            {
                AboutWindow aboutWindow = new AboutWindow();
                aboutWindow.Show();
                Common.isAboutWindowShow = true;
            }
        }
        void menuExit_Click(object sender,EventArgs e)
        {
            Exit();
        }

        void SBExit_Completed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Exit()
        {
            notifyIcon.Dispose();
            SBExit.Completed += new EventHandler(SBExit_Completed);
            SBExit.Begin();
        }
    }
}

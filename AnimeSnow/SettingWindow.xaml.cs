using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace AnimeSnow
{
	/// <summary>
	/// SettingWindow.xaml 的交互逻辑
	/// </summary>
	public partial class SettingWindow : Window
	{
		
		MainWindow mainWindow;
		string strName = "桂叶雪花飘落动态桌面";
		public SettingWindow(MainWindow mw)
		{
			this.InitializeComponent();
			this.Closed += new EventHandler(SettingWindow_Closed);
			mainWindow = mw;

			//slider1、2
			sliderMiddium.Value = mainWindow.GridMiddium.Opacity;
			sliderSmall.Value = mainWindow.GridSmall.Opacity;
			sliderMiddium.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderMiddium_ValueChanged);
			sliderSmall.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderSmall_ValueChanged);

			//sliderspeed
			sliderSpeed.Value = mainWindow.SBMiddiumSnow.SpeedRatio;
			sliderSpeed.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderSpeed_ValueChanged);

			//置顶
			if (mainWindow.Topmost)
				comboboxWindowState.SelectedIndex = 0;
			else
				comboboxWindowState.SelectedIndex = 1;
			comboboxWindowState.SelectionChanged += new SelectionChangedEventHandler(comboboxWindowState_SelectionChanged);

			//开机启动
			if (isRunStartup())
				checkboxRunStartup.IsChecked = true;
			else
				checkboxRunStartup.IsChecked = false;
			checkboxRunStartup.Click += new RoutedEventHandler(checkboxRunStartup_Click);

			
		}

		void sliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			mainWindow.SBMiddiumSnow.SetSpeedRatio(sliderSpeed.Value);
			mainWindow.SBSmallSnow.SetSpeedRatio(sliderSpeed.Value);
		}

		void checkboxRunStartup_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.RegistryKey rootKey = Microsoft.Win32.Registry.CurrentUser;//本地计算机数据的配置 
			Microsoft.Win32.RegistryKey runKey = rootKey.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

			if (checkboxRunStartup.IsChecked.Value)//创建开机启动 
			{
				try
				{
					runKey.SetValue(strName, Process.GetCurrentProcess().MainModule.FileName);
					rootKey.Close();// 刷新 关闭 保存修改 
				}
				catch { }
			}
			else//关闭开机启动
			{
				try
				{
					runKey.DeleteValue(strName);
					rootKey.Close();
				}
				catch { }
			}
		}

		void comboboxWindowState_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			mainWindow.SetWindowPosition(comboboxWindowState.SelectedIndex);
		}

		void sliderMiddium_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			mainWindow.GridMiddium.Opacity = sliderMiddium.Value;
		}

		void sliderSmall_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			mainWindow.GridSmall.Opacity = sliderSmall.Value;
		}

		void SettingWindow_Closed(object sender, EventArgs e)
		{
			Setting setting = new Setting();
			setting.Opacity1 = sliderMiddium.Value;
			setting.Opacity2 = sliderSmall.Value;
			setting.Speed = sliderSpeed.Value;
			setting.WindowPos = comboboxWindowState.SelectedIndex;
			setting.Save();
			Common.isSettingWindowShow = false;
		}

		bool isRunStartup()
		{
			Microsoft.Win32.RegistryKey rootKey = Microsoft.Win32.Registry.CurrentUser;//本地计算机数据的配置 
			Microsoft.Win32.RegistryKey runKey = rootKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
			try
			{
				if (runKey.GetValue(strName) != null)
					return true;
				else
					return false;
			}
			catch { return false; }
		}//判断是否已经开机启动
	}
}
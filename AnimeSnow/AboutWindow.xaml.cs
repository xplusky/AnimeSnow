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

namespace AnimeSnow
{
	/// <summary>
	/// AboutWindow.xaml 的交互逻辑
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			this.InitializeComponent();
            this.Closed += new EventHandler(AboutWindow_Closed);
			// 在此点之下插入创建对象所需的代码。
		}

        void AboutWindow_Closed(object sender, EventArgs e)
        {
            Common.isAboutWindowShow = false;
        }
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MoonSliver.Code;
using Panuon.UI.Silver;
using PropertyChanged;

namespace MoonSliver.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        [AddINotifyPropertyChangedInterface]
        public class MainWM : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public Setting Setting { get; set; }
        }

        public MainWM VM = new MainWM();
        public MainWindow()
        {
            InitializeComponent();
            VM.Setting = MoonSliver.Code.MoonSliver.Settings;
            DataContext = VM;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MoonSliver.Code.MoonSliver.Settings = VM.Setting;
            MoonSliver.Code.MoonSliver.Save(MenuCaller.api.AppDirectory);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dir = System.IO.Path.Combine(MenuCaller.api.AppDirectory, "Keywords");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            Process.Start("explorer.exe", dir);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var p = PendingBoxX.Show("正在连接网络...");
            try
            {
                var url = "https://gitee.com/free2048/textfilter/raw/master/keywords";
                p.UpdateMessage("正在下载敏感词库...\n请勿用于违规途径，否则追究法律责任。");
                var cc = await new HttpClient().GetAsync(url);
                var str = await cc.Content.ReadAsStringAsync();
                p.UpdateMessage("获取内容...\n请勿用于违规途径,，否则依法追究责任");

                var file = System.IO.Path.Combine(MenuCaller.api.AppDirectory, "Keywords", "words.txt");

                if (!File.Exists(file))
                    File.Create(file).Close();

                File.WriteAllText(file,str);
                p.UpdateMessage("内容获取完成，写出文件成功！...\n请勿用于违规途径,，否则依法追究责任");
                await Task.Delay(2000);
                
                p.Close();

            }
            catch (Exception ex)
            {

                p.UpdateMessage("失败...请联系开发解决问题\n" + "问题原因:" + ex.Message);
            }
        }
    }
}

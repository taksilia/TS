using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace TSUL
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isInfo;
        private static WebClient client;
        private string curDurect;
        private string adr;
        private string ver;
        private string infon;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Load(object sender, RoutedEventArgs e)
        {
            curDurect = Environment.CommandLine;
            curDurect = curDurect.Substring(1, curDurect.Length - 12);
            string[] ln = File.ReadAllLines(curDurect + "/lver");
            adr = ln[2];
            ver = ln[3];
            for(int i = 4; i < ln.Length; i++)
            {
                infon += ln[i] + "\n";
            }
            boxinfo.Text = infon;
            Cheec();
        }
        private void Cheec()
        {
            if (File.Exists(curDurect + "/gver"))
            {
                string a = File.ReadAllLines(curDurect + "/gver")[0];
                if(a == ver)
                {
                    StartBitton.Content = "Играть";
                    ProgressText.Text = "Установлено последнее обновление";
                    Bar.IsIndeterminate = false;
                    Bar.Value = 100;
                    TextVersion.Text = "- v" + ver;
                }
                else
                {
                    StartBitton.Content = "Обновить";
                    ProgressText.Text = "Обновите игру ->";
                    Bar.IsIndeterminate = false;
                }
            }
            else
            {
                StartBitton.Content = "Установить";
                ProgressText.Text = "Запустите установку ->";
                Bar.IsIndeterminate = false;
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        public void Ex(object sender, RoutedEventArgs rea)
        {
            Close();
        }
        public void SW(object sender, RoutedEventArgs rea)
        {
            WindowState = WindowState.Minimized;
        }
        public void poch()
        {
            Directory.Delete(curDurect + "/TScraft", true);
            Thread.Sleep(500);
            ZipFile.ExtractToDirectory(curDurect + "/TScraft.zip", curDurect + "/TScraft");
            Cheec();
        }
        public void IsLoad(object sender, RoutedEventArgs rea)
        {
            if ((string)StartBitton.Content == "Играть")
            {
                Process.Start(curDurect + "/TScraft/TScraft.exe");
                Close();
            }
            else if ((string)StartBitton.Content == "Обновить")
            {
                Directory.Delete(curDurect + "/TScraft", true);
                Thread.Sleep(500);
                if (client == null || !client.IsBusy)
                {
                    client = new WebClient();
                    client.DownloadFileCompleted += DowGameComp;
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DowGameBar);
                    client.DownloadFileAsync(new Uri(adr), curDurect + "/TScraft.zip");
                }
            }
            else if ((string)StartBitton.Content == "Установить")
            {
                if (client == null || !client.IsBusy)
                {
                    StartBitton.Content = "...";
                    client = new WebClient();
                    client.DownloadFileCompleted += DowGameComp;
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DowGameBar);
                    client.DownloadFileAsync(new Uri(adr), curDurect + "/TScraft.zip");
                }
            }
        }
        private void DowGameComp(object sender, AsyncCompletedEventArgs e)
        {
            ProgressText.Text = "Инициализация...";
            Bar.IsIndeterminate = true;
            ZipFile.ExtractToDirectory(curDurect + "/TScraft.zip", curDurect + "/TScraft");
            File.WriteAllText(curDurect + "/gver", ver);
            Cheec();
        }
        private void DowGameBar(object sender, DownloadProgressChangedEventArgs e)
        {
            Bar.Value = e.ProgressPercentage;
            ProgressText.Text = "Скачано " + ((int)e.BytesReceived / 1048576) + " мб из " + ((int)e.TotalBytesToReceive / 1048576) + " мб   " + e.ProgressPercentage + "%";
        }
        private void ninfo(object sender, RoutedEventArgs e)
        {
            if (isInfo)
            {
                isInfo = false;
                boxinfo.Visibility = Visibility.Hidden;
                info.Margin = new Thickness(0, 284, 0, 0);
            }
            else
            {
                isInfo = true;
                boxinfo.Visibility = Visibility.Visible;
                info.Margin = new Thickness(205, 284, 0, 0);
            }
        }
    }
}

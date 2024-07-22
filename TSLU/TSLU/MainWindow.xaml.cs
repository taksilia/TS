using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
namespace TSLU
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static WebClient client;
        private bool isDev;
        private string curDurect;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                curDurect = Environment.CommandLine;
                curDurect = curDurect.Substring(1, curDurect.Length - 12);
                if (File.Exists(curDurect + "/a09far"))
                {
                    isDev = true;
                    if (File.Exists(curDurect + "/TSDL.exe"))
                    {
                        if (File.Exists(curDurect + "/lver"))
                        {
                            if (client == null || !client.IsBusy)
                            {
                                client = new WebClient();
                                client.DownloadFileCompleted += UpdateLauncher;
                                client.DownloadFileAsync(new Uri("https://raw.githubusercontent.com/taksilia/TS/main/sver"), curDurect + "/sver");
                            }
                        }
                        else
                        {
                            DownloadLauncher_();
                        }
                    }
                    else
                    {
                        DownloadLauncher_();
                    }
                }
                else
                {
                    isDev = false;
                    if (File.Exists(curDurect + "/TSUL.exe"))
                    {
                        if (File.Exists(curDurect + "/lver"))
                        {
                            if (client == null || !client.IsBusy)
                            {
                                client = new WebClient();
                                client.DownloadFileCompleted += UpdateLauncher;
                                client.DownloadFileAsync(new Uri("https://raw.githubusercontent.com/taksilia/TS/main/sver"), curDurect + "/sver");
                            }
                        }
                        else
                        {
                            DownloadLauncher_();
                        }
                    }
                    else
                    {
                        DownloadLauncher_();
                    }
                }
            }
            catch (Exception a)
            {
                Close();
                MessageBox.Show(a + "");
                LateStartingGame();
            }
        }

        private void UpdateLauncher(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string[] sln = File.ReadAllLines(curDurect + "/sver");
                string[] lln = File.ReadAllLines(curDurect + "/lver");
                if (isDev)
                {
                    if (sln[0] == lln[0])
                    {
                        Process.Start(curDurect + "/TSDL.exe");
                        Close();
                    }
                    else
                    {
                        DownloadLauncher_();
                    }
                }
                else
                {
                    if (sln[1] == lln[1])
                    {
                        Process.Start(curDurect + "/TSUL.exe");
                        Close();
                    }
                    else
                    {
                        DownloadLauncher_();
                    }
                }
            }
            catch (Exception b)
            {
                Close();
                MessageBox.Show(b + "");
                LateStartingGame();
            }
        }
        private void DownloadLauncher_()
        {
            try
            {
                Create(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/TScraft.lnk", curDurect + "/TSLU.exe");
                Create(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "/TScraft.lnk", curDurect + "/TSLU.exe");
                if (client == null || !client.IsBusy)
                {
                    client = new WebClient();
                    client.DownloadFileCompleted += DownloadLauncher;
                    client.DownloadFileAsync(new Uri("https://raw.githubusercontent.com/taksilia/TS/main/sver"), curDurect + "/lver");
                }
            }
            catch (Exception c)
            {
                Close();
                MessageBox.Show(c + "");
                LateStartingGame();
            }
        }
        private void DownloadLauncher(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (client == null || !client.IsBusy)
                {
                    if (isDev)
                    {
                        client = new WebClient();
                        client.DownloadFileCompleted += StartLauncher;
                        client.DownloadFileAsync(new Uri("https://github.com/taksilia/TS/raw/main/TSDL.exe"), curDurect + "/TSDL.exe");
                    }
                    else
                    {
                        client = new WebClient();
                        client.DownloadFileCompleted += StartLauncher;
                        client.DownloadFileAsync(new Uri("https://github.com/taksilia/TS/raw/main/TSUL.exe"), curDurect + "/TSUL.exe");
                    }
                }
            }
            catch (Exception d)
            {
                Close();
                MessageBox.Show(d + "");
                LateStartingGame();
            }
        }
        private void StartLauncher(object sender, AsyncCompletedEventArgs e)
        {
            if (isDev)
            {
                Process.Start(curDurect + "/TSDL.exe");
                Close();
            }
            else
            {
                Process.Start(curDurect + "/TSUL.exe");
                Close();
            }
        }
        private static void Create(string ShortcutPath, string TargetPath)
        {
            IWshRuntimeLibrary.WshShell wshShell = new IWshRuntimeLibrary.WshShell(); //создаем объект wsh shell

            IWshRuntimeLibrary.IWshShortcut Shortcut = (IWshRuntimeLibrary.IWshShortcut)wshShell.
                CreateShortcut(ShortcutPath);

            Shortcut.TargetPath = TargetPath; //путь к целевому файлу

            Shortcut.Save();
        }
        private void LateStartingGame()
        {
            if (Directory.Exists(curDurect + "/TScraft"))
            {
                Process.Start(curDurect + "/TScraft/TScraft.exe");
            }
        }
    }
}

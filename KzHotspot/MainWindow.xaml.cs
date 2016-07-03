using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Shapes;
using System.Xml;
using Ionic.Zip;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace KzHotspot
{

    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string _v = 103.ToString();

        private void OnOffBt_OnClick(object sender, RoutedEventArgs e)
        {
           Process islem = new Process();
            Process baslat = new Process();
            islem.StartInfo.FileName = "cmd.exe";
            islem.StartInfo.Arguments = "/C netsh wlan set hostednetwork mode=allow ssid="+IdTx.Text+" key="+SifreTx;
            islem.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            islem.Start();
            islem.Close();
            if (OnOffBt.Content.ToString() == "Başlat")
            {
                baslat.StartInfo.FileName = "cmd.exe";
                baslat.StartInfo.Arguments = "/C netsh wlan start hostednetwork";
                baslat.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                baslat.Start();
                baslat.Close();
                OnOffBt.Content = "Durdur";
                this.ShowMessageAsync("Bilgi", "Hotspot Açıldı");
            }
            else
            {
                baslat.StartInfo.FileName = "cmd.exe";
                baslat.StartInfo.Arguments = "/C netsh wlan stop hostednetwork";
                baslat.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                baslat.Start();
                baslat.Close();
                OnOffBt.Content = "Başlat";
                this.ShowMessageAsync("Bilgi", "Hotspot Kapatıldı");
            }

            Registry.CurrentUser.CreateSubKey("Software")?.CreateSubKey("KzSoftware")?.SetValue("SSID", IdTx.Text);
            Registry.CurrentUser.CreateSubKey("Software")?.CreateSubKey("KzSoftware")?.SetValue("Sifre", SifreTx.Text);
        }


        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var baslangic = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    Arguments = "/C netsh wlan stop hostednetwork",
                    WindowStyle = ProcessWindowStyle.Hidden,
                }
            }; //giriş kontrol
            baslangic.Start();
            baslangic.Close();
            Process sil = new Process();
            sil.StartInfo.FileName = "cmd.exe";
            sil.StartInfo.Arguments = "/C RMDIR \"Temp\" /S /Q ";
            sil.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            sil.Start();
            Process sil2 = new Process();
            sil2.StartInfo.FileName = "cmd.exe";
            sil2.StartInfo.Arguments = "/C DEL KzHotspot.zip /S /Q";
            sil2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            sil2.Start();

            if (Registry.CurrentUser.OpenSubKey("Software")?.OpenSubKey("KzSoftware")?.GetValue("SSID").ToString() != null)
            {
                IdTx.Text = Registry.CurrentUser.CreateSubKey("Software")?.OpenSubKey("KzSoftware")?.GetValue("SSID").ToString();
                SifreTx.Text = Registry.CurrentUser.CreateSubKey("Software")?.OpenSubKey("KzSoftware")?.GetValue("Sifre").ToString();
            }
            else
            {
                Registry.CurrentUser.CreateSubKey("Software")?.CreateSubKey("KzSoftware")?.SetValue("SSID", "KzHotspot");
                Registry.CurrentUser.CreateSubKey("Software")?.CreateSubKey("KzSoftware")?.SetValue("Sifre", "kzhotspot");
            }
#region Güncelleme
            var wc = new WebClient {Proxy = null};
            try
            {
                var guncelleme =
                    wc.DownloadString("https://raw.githubusercontent.com/Adilx05/KzHotspot/master/version.txt");
                if (guncelleme != _v)
                {

                    using (var client = new WebClient())
                    {
                        var controller = await this.ShowProgressAsync("Lütfen Bekleyin","Güncelleme İndiriliyor");
                        client.DownloadFile("https://github.com/Adilx05/KzHotspot/raw/master/KzHotspot/Bin/Debug/KzHotspot.zip","KzHotspot.zip");
                        client.DownloadFileCompleted += Client_DownloadFileCompleted;
                        string cikarilacak = "KzHotspot.zip";
                        using (ZipFile zip1 = ZipFile.Read(cikarilacak))
                        {
                            foreach (ZipEntry s in zip1)
                            {
                                s.Extract("Temp",ExtractExistingFileAction.OverwriteSilently);
                            }
                        }
                        await controller.CloseAsync();
                    }

                    await this.ShowMessageAsync("Bilgi", "Güncelleme Tamamlandı Lütfen Programı Yeniden Başlatın");

                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("Bilgi", "Güncelleme Dosyalarına Ulaşılamadı. Lütfen İnternet Bağlantınızı Kontrol Edin!");
            }


            #endregion Güncelleme
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            

        }


        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            
            Process tasi = new Process();
          //  Process sil = new Process();

            tasi.StartInfo.FileName = "cmd.exe";
            tasi.StartInfo.Arguments = "/C XCOPY Temp\\* /y";
            tasi.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            tasi.Start();

         /*   sil.StartInfo.FileName = "cmd.exe";
            sil.StartInfo.Arguments = "/C RMDIR \"Temp\" /S /Q ";
            sil.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            sil.Start();*/


        }
    }

    
}

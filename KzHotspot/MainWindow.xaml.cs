using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security.AccessControl;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private void OnOffBt_OnClick(object sender, RoutedEventArgs e)
        {
           Process Islem = new Process();
            Process Baslat = new Process();
            Islem.StartInfo.FileName = "cmd.exe";
            Islem.StartInfo.Arguments = "/C netsh wlan set hostednetwork mode=allow ssid="+IdTx.Text+" key="+SifreTx;
            Islem.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Islem.Start();
            Islem.Close();
            if (OnOffBt.Content.ToString() == "Başlat")
            {
                Baslat.StartInfo.FileName = "cmd.exe";
                Baslat.StartInfo.Arguments = "/C netsh wlan start hostednetwork";
                Baslat.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Baslat.Start();
                Baslat.Close();
                OnOffBt.Content = "Durdur";
                this.ShowMessageAsync("Bilgi", "Hotspot Açıldı");
            }
            else
            {
                Baslat.StartInfo.FileName = "cmd.exe";
                Baslat.StartInfo.Arguments = "/C netsh wlan stop hostednetwork";
                Baslat.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Baslat.Start();
                Baslat.Close();
                OnOffBt.Content = "Başlat";
                this.ShowMessageAsync("Bilgi", "Hotspot Kapatıldı");
            }

            Registry.CurrentUser.OpenSubKey("Software")?.CreateSubKey("KzSoftware")?.SetValue("SSID", IdTx.Text);
            Registry.CurrentUser.OpenSubKey("Software")?.CreateSubKey("KzSoftware")?.SetValue("Sifre", SifreTx.Text);
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
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
        }
    }

    
}

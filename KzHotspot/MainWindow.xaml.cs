﻿using System;
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
            Islem.StartInfo.Arguments = "netsh wlan set hostednetwork mode=allow ssid="+IdTx.Text+" key="+SifreTx;
            Islem.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Islem.Start();
            Islem.WaitForExit();
            if (OnOffBt.Content.ToString() == "Başlat")
            {
                Baslat.StartInfo.FileName = "cmd.exe";
                Baslat.StartInfo.Arguments = "netsh wlan start hostednetwork";
                Baslat.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Baslat.Start();
                Baslat.WaitForExit();
                OnOffBt.Content = "Durdur";
                this.ShowMessageAsync("Bilgi", "Hotspot Açıldı");
            }
            else
            {
                Baslat.StartInfo.FileName = "cmd.exe";
                Baslat.StartInfo.Arguments = "netsh wlan stop hostednetwork";
                Baslat.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Baslat.Start();
                Baslat.WaitForExit();
                OnOffBt.Content = "Başlat";
                this.ShowMessageAsync("Bilgi", "Hotspot Kapatıldı");
            }

            Registry.CurrentUser.OpenSubKey("Software")?.CreateSubKey("KzSoftware")?.SetValue("SSID", IdTx.Text);
            Registry.CurrentUser.OpenSubKey("Software")?.CreateSubKey("KzSoftware")?.SetValue("Sifre", SifreTx.Text);
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Process Baslangic = new Process(); //giriş kontrol
            Baslangic.StartInfo.FileName = "cmd.exe";
            Baslangic.StartInfo.Arguments = "netsh wlan stop hostednetwork";
            Baslangic.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Baslangic.Start();
            Baslangic.WaitForExit();

            if (Registry.CurrentUser.OpenSubKey("Software")?.OpenSubKey("KzSoftware")?.GetValue("SSID").ToString() != null)
            {
                IdTx.Text = Registry.CurrentUser.OpenSubKey("Software")?.OpenSubKey("KzSoftware")?.GetValue("SSID").ToString();
                SifreTx.Text = Registry.CurrentUser.OpenSubKey("Software")?.OpenSubKey("KzSoftware")?.GetValue("Sifre").ToString();
            }
            else
            {
                Registry.CurrentUser.OpenSubKey("Software")?.CreateSubKey("KzSoftware")?.SetValue("SSID", "KzHotspot");
                Registry.CurrentUser.OpenSubKey("Software")?.CreateSubKey("KzSoftware")?.SetValue("Sifre", "kzhotspot");
            }
        }
    }

    
}
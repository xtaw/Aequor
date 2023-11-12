using Aequor.Audio;
using Aequor.Main;
using Aequor.Util;
using CefSharp;
using CefSharp.Handler;
using CefSharp.Structs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Aequor.UI
{

    public partial class ShanBay : Window
    {

        public bool Locked { get; private set; }

        public ShanBay()
        {
            InitializeComponent();
            Browser.RequestHandler = new AudioRequestHandler();
            Browser.Address = "https://web.shanbay.com/wordsweb/#/study/entry";
        }

        public void Lock()
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            WindowState = WindowState.Maximized;
            Topmost = true;
            ShowInTaskbar = false;
            Locked = true;
            ToggleLockButton.Content = FindResource("UnlockIcon");
            ToggleLockButton.ToolTip = "解锁";
        }

        public void Unlock()
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            ResizeMode = ResizeMode.CanResize;
            WindowState = WindowState.Normal;
            Topmost = false;
            ShowInTaskbar = true;
            Locked = false;
            ToggleLockButton.Content = FindResource("LockIcon");
            ToggleLockButton.ToolTip = "锁定";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            if (!Locked)
            {
                Hide();
            }
        }

        private void ToggleLock(object sender, RoutedEventArgs e)
        {
            if (Locked)
            {
                IsEnabled = false;
                new Task(() =>
                {
                    if (!AequorClient.INSTANCE.configManager.forceStudy || ShanBayUtil.IsStudyFinished())
                    {
                        Dispatcher.Invoke(() =>
                        {
                            Unlock();
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DialogHost.Show(ErrorDialog.DialogContent!, "ErrorDialog");
                        });
                    }
                    Dispatcher.Invoke(() =>
                    {
                        IsEnabled = true;
                    });
                }).Start();
            }
            else
            {
                Lock();
            }
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            Browser.Address = "https://web.shanbay.com/wordsweb/#/study/entry";
            Browser.Reload();
        }
    }

}

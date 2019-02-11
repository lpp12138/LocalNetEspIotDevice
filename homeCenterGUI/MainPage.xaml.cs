using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace homeCenterGUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void MyNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            Debug.WriteLine("invoked");
                NavigationViewItem invokedItem= sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                switch (invokedItem.Tag)
                {
                    case "nav_Home":
                        ContentFrame.Navigate(typeof(PageFolder.HomePage),null);
                    break;
                    case "nav_Page1":
                        ContentFrame.Navigate(typeof(PageFolder.Page1), null);
                        break;
                    case "nav_Page2":
                        ContentFrame.Navigate(typeof(PageFolder.Page2), null);
                        break;
                    case "nav_AboutPage":
                        ContentFrame.Navigate(typeof(PageFolder.AboutPage), null);
                        break;
                }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine("navFailed");
        }

        private void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e) => Debug.WriteLine("naving");

        private void My_Nav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            Application.Current.Exit();
        }
    }
}

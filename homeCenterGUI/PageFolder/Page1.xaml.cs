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
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Popups;
using System.Diagnostics;
using Windows.Data.Json;
using Windows.Storage.Streams;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace homeCenterGUI.PageFolder
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Page1 : Page
    {
        StreamSocketListener listener;
        public Page1()
        {
            this.InitializeComponent();
        }
        private async void  SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SwitchButton.Content.ToString() == "Start")
            {
                if (listener != null)
                {
                    await new MessageDialog("listening has been started").ShowAsync();
                    return;
                }
                listener = new StreamSocketListener();
                listener.ConnectionReceived += Listener_ConnectionReceived;
                try
                {
                    await listener.BindServiceNameAsync("2334");
                    await new MessageDialog("listening started").ShowAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                SwitchButton.Content = "Stop";
            }
            else
            {
                listener.Dispose();
                listener = null;
                SwitchButton.Content = "Start";
            }     
        }

        private void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            try
            {
                string TcpData = "";
                //获取tcp输入流，当tcp连接结束时，流也随之结束
                using (StreamReader streamReader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
                {
                    while (!streamReader.EndOfStream)
                    {
                        TcpData += streamReader.ReadLine();
                        Debug.WriteLine(TcpData);
                    }
                }
                SolveTcpData(TcpData);
            }
            catch (Exception ex)
            {
                listener.Dispose();
                listener = null;
                Debug.WriteLine(ex.Message);
            }
        }

        private async void SolveTcpData(String TcpData)
        {
            JsonObject jsonData;
            if(JsonObject.TryParse(TcpData,out jsonData))
            {
                Debug.WriteLine("JsonObjet parse succeed");
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,async ()=> await new MessageDialog(jsonData.ToString()).ShowAsync());
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("RefreshButton_Click");
            string data = "{\"protcol\":\"myEspNet\",\"command\":\"ping\"}";
            DatagramSocket datagramSocket = new DatagramSocket();
            IOutputStream outputStream = await datagramSocket.GetOutputStreamAsync(new HostName("255.255.255.255"), "2333");
            DataWriter dataWriter = new DataWriter(outputStream);
            Debug.WriteLine(dataWriter.WriteString(data));
            await dataWriter.StoreAsync();
            datagramSocket.Dispose();
            datagramSocket = null;
        }
    }
}
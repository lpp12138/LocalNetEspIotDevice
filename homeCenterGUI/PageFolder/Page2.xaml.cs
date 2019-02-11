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
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Storage.Streams;
using System.Diagnostics;
using Windows.UI.Popups;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace homeCenterGUI.PageFolder
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Page2 : Page
    {
        StreamSocketListener listener;
        public Page2()
        {
            this.InitializeComponent();
        }

        private async void UDP_Send_Button_Click(object sender, RoutedEventArgs e)
        {
            string data = UdpDataTextBox.Text;
            UdpDataTextBox.Text = "";
            DatagramSocket datagramSocket = new DatagramSocket();
            IOutputStream outputStream = await datagramSocket.GetOutputStreamAsync(new HostName("255.255.255.255"), "2333");
            DataWriter dataWriter = new DataWriter(outputStream);
            dataWriter.WriteString(data);
            await dataWriter.StoreAsync();
            datagramSocket.Dispose();
            datagramSocket = null;
        }

        private void DatagramSocket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private async void ServerStartButton_Click(object sender, RoutedEventArgs e)
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
                await listener.BindServiceNameAsync(TcpPortTextBox.Text);
                await new MessageDialog("listening started").ShowAsync();
                TcpReceiveTextbox.Text = "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            try
            {
                //获取tcp输入流，当tcp连接结束时，流也随之结束
                using (StreamReader streamReader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string request = streamReader.ReadLine();
                        Debug.WriteLine(request);
                        if (request != null)
                        {
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => TcpReceiveTextbox.Text += $"From:{args.Socket.Information.RemoteHostName} Received:{request}\r\n");
                        }
                    }
                    listener.Dispose();
                    listener = null;
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => TcpReceiveTextbox.Text = "Tcp连接断开，服务器已停止");
                }
            }
            catch(Exception ex)
            {
                listener.Dispose();
                listener = null;
                Debug.WriteLine(ex.Message);
            }
        }
    }
}

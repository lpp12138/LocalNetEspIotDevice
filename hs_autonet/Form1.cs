/*
 * 作者：lpp
 * lpp12138@outlook.com
 * udp-tcp短连接自组网上报系统
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace hs_autonet
{
    public partial class Form1 : Form
    {
        const int myUdpPort = 2333;
        const int myTcpPort = 2334;
        public Form1()
        {
            InitializeComponent();
        }
        static byte[] buffer = new byte[1024];
        /*
         * 为了处理静态异步回调方法中接收的客户端数据，使用监视线程和静态旗标处理invoke调用
         * 需要刷新数据时将数据放至refershData并将needDataRefersh设为true
         */
        static bool needDataRefersh = false;
        static String refershData = null;
        public void RefershLog()
        {
            while(true)
            {
                if (needDataRefersh == true)
                {
                    this.Invoke(new Action(() => {
                        this.logBox.Text += refershData + "\r\n";
                    }));
                    //回头这里再加一个上报OneNet服务器
                    needDataRefersh = false;
                }
            }
        }
        /*
         * 计时器触发，UDP广播目前本机ip和接受TCP连接的端口号
         * 注意只能在同网段中广播
         */
        private void UdpTimer_Tick(object sender, EventArgs e)
        {
            //IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress[] ipAddr = ipHost.AddressList;
            //refershData = ipHost.AddressList.Length.ToString();
           // needDataRefersh = true;
           // for (int index=0;index<ipAddr.Length;index++)
            {
               // MessageBox.Show(ipAddr[index].ToString());
               // if (ipAddr[index].AddressFamily == AddressFamily.InterNetwork)
                {
                    //string ip = ipAddr[index].ToString();
                    IPEndPoint ipEndPort = new IPEndPoint(IPAddress.Broadcast, myUdpPort);
                    UdpClient udpClient = new UdpClient(2333);
                    String data = "{\"protocol\":\"myEspNet\"}";
                    udpClient.Send(Encoding.ASCII.GetBytes(data),Encoding.ASCII.GetByteCount(data),ipEndPort);
                    udpClient.Dispose();
                    logBox.Text += "udp pack sended\r\n";
                }
            }
        }
        //tested
        private void Form1_Load(object sender, EventArgs e)
        {
            udpTimer.Interval = (int)TimerTime.Value;
            logBox.Text += "Started UDP Broadcast"+"\r\n";
            //监视线程
            Thread logRefershThread = new Thread(RefershLog);
            logRefershThread.Start();
            //socket监听操作
            Socket serverSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPAddress iPAddress = IPAddress.Parse("0.0.0.0");
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress,2334);
            serverSocket.Bind(iPEndPoint);
            serverSocket.Listen(15);
            serverSocket.BeginAccept(SocketAcceptAsycCallback,serverSocket);
        }
        //接受TCP连接
        static void SocketAcceptAsycCallback(IAsyncResult asyncResult)
        {
            Socket serverSocket = asyncResult.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(asyncResult);
            clientSocket.BeginReceive(buffer,0,1024,SocketFlags.None,SocketReceiveCallback,clientSocket);
            serverSocket.BeginAccept(SocketAcceptAsycCallback,serverSocket);
        }
        //处理TCP短连接数据接收
        private static void SocketReceiveCallback(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int datalength =clientSocket.EndReceive(ar);
                //如果客户端断开，主机也关闭对应的socket端口
                if(datalength==0)
                {
                    clientSocket.Close();
                }
                String message = Encoding.ASCII.GetString(buffer,0,datalength);
                // MessageBox.Show(message);
                // RecData recData = JsonConvert.DeserializeObject<RecData>(message);
                if (message!="")//recData.alert)
                {
                    needDataRefersh = true;
                    refershData = message;// recData.deviceName + ":" + recData.alert;
                }
                clientSocket.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                clientSocket.Close();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }
        //设置timer的打点时间，没什么好说的
        private void TimerTime_ValueChanged(object sender, EventArgs e)
        {
            udpTimer.Interval = (int)TimerTime.Value;
        }

        private void LogBox_DoubleClick(object sender, EventArgs e)
        {
            logBox.Text = "";
        }
    }
    //接受的数据格式
    public class RecData
    {
        public String deviceName { get; set; }
        public bool alert { get; set; }
    }
}

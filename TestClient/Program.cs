using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Net;
using System.Windows.Forms;
using System.Net.Sockets;
namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
            //    //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //    //var msg = new Data() { Command = Command.Login, Message = "hello", ClientLogin = "wweor" }.ToBytes();

            //    //EndPoint server = new IPEndPoint(IPAddress.Loopback, 1000);

            //    //s.BeginSendTo(msg, 0, msg.Length, SocketFlags.None, server, OnSend, null);

            //}
            //catch (Exception e) 
            //{
                
            //}
            ChatClient asdfas = new ChatClient("asdf");

            

            Application.Run(new ApplicationContext());
        }

        private static void OnSend(IAsyncResult ar)
        {
            try
            {
                throw new NotImplementedException();

            }
            catch (Exception) { }
        }
    }
}

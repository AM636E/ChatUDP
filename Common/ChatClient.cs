using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Common
{
    public class ChatClient
    {
        private Socket _client;
        private string _login;
        private EndPoint _server;
        private byte[] _data = new byte[1024];
        public byte[] Data { get { return _data; } }
        public event EventHandler Received;

        /// <summary>
        /// Create a new ChatClient login and start listen to the server
        /// </summary>
        /// <param name="login">Login in the room</param>
        /// <param name="ip">Ip of server</param>
        /// <param name="port">Port of server</param>
        public ChatClient(string login, string ip = "127.0.0.1", int port = 1000)
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var msg = new Data() { Command = Command.Login, Message = "hello", ClientLogin = login }.ToBytes();

            EndPoint server = new IPEndPoint(IPAddress.Loopback, 1000);
            _server = server;
            _login = login;
            _client.BeginSendTo(msg, 0, msg.Length, SocketFlags.None, _server, OnSend, null);
            Login();
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                var a = Received;
                if(a != null)
                {
                    a(this, EventArgs.Empty);
                }
                Data d = new Data(_data);
                Console.WriteLine(d);
               // Console.WriteLine(Encoding.UTF8.GetString(_data));
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SendData(Data data)
        {
            byte[] message = data.ToBytes();

            _client.BeginSendTo(message, 0, message.Length, SocketFlags.None, _server, OnSend, null);
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                _client.EndSendTo(ar);
                Console.WriteLine("staring to listenint the server");
                _client.BeginReceiveFrom(_data, 0, _data.Length, SocketFlags.None, ref _server, OnReceive, null);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void Login()
        {
            
        }
    }
}

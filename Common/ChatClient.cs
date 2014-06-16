using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Common
{
    public class ChatEventArgs : EventArgs
    {
        public Data received { get; set; }
    }
    public class ChatClient
    {
        private Socket _client;
        private string _login;
        private EndPoint _server;
        private byte[] _data = new byte[1024];
        public byte[] Data { get { return _data; } }
        public delegate void ChatEventHandler(object o, ChatEventArgs e);
        public event ChatEventHandler Received;
        private int _loginCount = 0;

        /// <summary>
        /// Create a new ChatClient instance.
        /// Login and start listen to the server.
        /// </summary>
        /// <param name="login">Login in the room</param>
        /// <param name="ip">Ip of server</param>
        /// <param name="port">Port of server</param>
        public ChatClient(string login, string ip = "127.0.0.1", int port = 1000)
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);            

            EndPoint server = new IPEndPoint(IPAddress.Loopback, 1000);
            _server = server;
            _login = login;
       
            Login();
        }

        private void OnReceive(IAsyncResult ar)
        {
                var a = Received;
                Data d = new Data(_data);
            if(d.Status != Status.OK)
            {
                throw new Exception("Something happens " + d.Status);
            }
                if(a != null)
                {
                    a(this, new ChatEventArgs() { received = d });
                }
                
                Console.WriteLine(d);
                switch(d.Status)
                {
                    case Status.LOGIN_TAKEN :
                        {
                            var index = _login.LastIndexOfAny("123456789".ToCharArray());
                            if(index == -1)
                            {
                                _login += (++_loginCount);
                            }
                            else
                            {
                                _login = _login.Substring(0, index) + (++_loginCount);
                            }
                            Login();
                            break;
                        }
                    case Status.RANDOM_SERVER_ERROR:
                        {
                            Login();
                            break;
                        }
                }
                _client.BeginReceiveFrom(_data, 0, _data.Length, SocketFlags.None, ref _server, OnReceive, null);
           
        }

        public void SendMessage(string message)
        {
            SendData(new Data() { Status = Status.OK, ClientLogin = _login, Message = message, Command = Command.Message });
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
            var msg = new Data() { Command = Command.Login, Message = "hello", ClientLogin = _login }.ToBytes();
            _client.BeginSendTo(msg, 0, msg.Length, SocketFlags.None, _server, OnSend, null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using Common;

namespace Chat
{
    public class ChatServer
    {
        private Socket _server = null;
        private List<ClientInfo> _clients = new List<ClientInfo>();
        const int SIZE = 1024;
        private byte[] _data = new byte[SIZE];
        private int _port;

        public List<ClientInfo> Clients
        {
            get { return _clients; }
        }
        public ChatServer(int port = 1000)
        {
            _port = port;

            Start();
        }

        public void Start()
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, _port);
                _server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _server.Bind(ipep);

                IPEndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint)remoteIp;
                _server.BeginReceiveFrom(_data, 0, _data.Length, SocketFlags.None, ref remote, OnReceive, null);

                Console.WriteLine("Server started and listening on the port {0}", _port);
    
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {           
            try
            {
                IPEndPoint ipepRemote = new IPEndPoint(IPAddress.Any, 0);
                EndPoint sender = (EndPoint)ipepRemote;
                
                _server.EndReceiveFrom(ar, ref sender);
                
                Data received = new Data(_data);
                Data toSend = null;
                Console.WriteLine("received {0}", received);
                switch(received.Command)
                {
                    case Command.Login:
                        {
                            if (IsLoginFree(received.ClientLogin))
                            {
                                _clients.Add(new ClientInfo() { Adress = sender, Login = received.ClientLogin });
                                toSend = new Data() { Status = Status.OK, Command = Command.Login, ClientLogin = "test", Message = "test" };
                            }
                            else
                            {
                                toSend = new Data() { Status = Status.LOGIN_TAKEN, Command = Command.Login, ClientLogin = "test", Message = "test" };
                            }
                            break;
                        }
                }

                byte[] message = toSend.ToBytes();
                _server.BeginSendTo(message, 0, message.Length, SocketFlags.None, sender, OnSend, null);
                _server.BeginReceiveFrom(_data, 0, SIZE, SocketFlags.None, ref sender, OnReceive, sender);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public bool IsLoginFree(string login)
        {
            return (from client in _clients
                    where client.Login == login
                    select client).Count() == 0;
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                _server.EndSend(ar);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

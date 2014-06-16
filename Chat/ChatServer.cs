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
                var client = new ClientInfo() { Login = received.ClientLogin, Adress = sender };
                switch(received.Command)
                {
                    case Command.Login:
                        {
                            if (IsLoginFree(received.ClientLogin))
                            {
                                _clients.Add(client);
                                SendDataToClient(client, Status.OK);
                                toSend = new Data() { Command = Command.None, Status = Status.OK, Message = received.ClientLogin + " has joined to the room", ClientLogin = "_login" };
                            }
                            else
                            {
                                SendDataToClient(client, Status.LOGIN_TAKEN);
                            }
                            break;
                        }
                    case Command.Message :
                        {
                            toSend = new Data() { Command = Command.Message, Status = Status.OK, Message = received.Message, ClientLogin = received.ClientLogin };
                            break;
                        }
                }
                if (toSend != null)
                {
                    SendDataToClients(toSend);
                }
                _server.BeginReceiveFrom(_data, 0, SIZE, SocketFlags.None, ref sender, OnReceive, sender);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public void SendDataToClients(Data data)
        {
            foreach(ClientInfo client in _clients)
            {
                SendDataToClient(client, data);
            }
        }

        public void SendDataToClients(Status status, Command command, string message = "fake", string login = "fake")
        {
            SendDataToClients(new Data() { Status = status, Command = command, Message = message, ClientLogin = login });
        }
        
        public void SendDataToClient(ClientInfo client, Data data )
        {
            if (data.ClientLogin == null)
            {
                data.ClientLogin = client.Login;
            }
            var message = data.ToBytes();
            _server.BeginSendTo(message, 0, message.Length, SocketFlags.None, client.Adress, OnSend, null);
        }

        public void SendDataToClient(ClientInfo client, Status status, Command command = Command.None, string message = "fake", string login = "fake")
        {
            SendDataToClient(client, new Data() { Status = status, Command = command, Message = message, ClientLogin = login });
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

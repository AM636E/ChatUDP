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
            ChatClient client = new ChatClient("asdf");

            client.Received += (o, e) =>
            {
                if (e.received.Command == Command.Message)
                {
                    Console.WriteLine("Message from {0} : {1}", e.received.ClientLogin, e.received.Message);
                }
            };

            client.SendMessage("Hi all!");

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

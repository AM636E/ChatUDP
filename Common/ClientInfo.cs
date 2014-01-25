using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Common
{
    public struct ClientInfo
    {
        public EndPoint Adress { get; set; }
        public string Login { get; set; }
    }
}

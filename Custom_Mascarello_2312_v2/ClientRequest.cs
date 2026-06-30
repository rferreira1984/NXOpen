using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Mascarello
{
    public class ClientRequest
    {
        public TcpClient TcpClient { get; set; }
        public DateTime RequestTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    internal class Model
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private string username;
        private List<String> connectedUser;
        private bool connected = false;
        

        public TcpClient Client { get => client; set => client = value; }
        public NetworkStream Stream { get => stream; set => stream = value; }
        public StreamReader Reader { get => reader; set => reader = value; }
        public StreamWriter Writer { get => writer; set => writer = value; }
        public string Username { get => username; set => username = value; }
        public bool Connected { get => connected; set => connected = value; }
        public List<string> ConnectedUser { get => connectedUser; set => connectedUser = value; }
    }
}

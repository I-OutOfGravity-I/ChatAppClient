using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ClientApp.Objects;

namespace ClientApp
{
    public class Controller
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private string username;
        private bool connected = false;
        private static Controller? instance;
        private static readonly object lockObject = new object();

        private Controller() { }

        public static Controller Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new Controller();
                        }
                    }
                }
                return instance;
            }
        }

        public bool Connected { get => connected; set => connected = value; }
        public string Username { get => username; set => username = value; }
        public StreamWriter Writer { get => writer; set => writer = value; }
        public StreamReader Reader { get => reader; set => reader = value; }
        public NetworkStream Stream { get => stream; set => stream = value; }
        public TcpClient Client { get => client; set => client = value; }

        public Exception? tryConnection(string server, string username)
        {
            Username = username;
            string serverIp = server;
            int serverPort = 12345;
            try
            { 
                client = new TcpClient(serverIp, serverPort);
                stream = client.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8);

                Message loginMessage =  new Message(MessageType.Login, username, "");
                string loginString = Message.SerializeMessage(loginMessage);
                writer.WriteLine(loginString);
                writer.Flush();


                connected = true;
                return null;
            }
            catch
            {
                return new Exception("Verbindung zu " + server + ":" + serverPort + " ist fehlgeschlagen");
            }
        }
    }
}

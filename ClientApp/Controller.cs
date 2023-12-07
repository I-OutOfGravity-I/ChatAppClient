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

namespace ClientApp
{
    public class Controller
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private bool connected = false;
        private static Controller instance;
        private static readonly object lockObject = new object();

        private Controller() { }

        // Public method to get the instance of the singleton class
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
        public StreamWriter Writer { get => writer; set => writer = value; }
        public StreamReader Reader { get => reader; set => reader = value; }

        public Exception tryConnection(string server, string username)
        {
            string serverIp = server;
            int serverPort = 12345; // Ändere den Port entsprechend deines Servers
            try
            { 
                client = new TcpClient(serverIp, serverPort);
                stream = client.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8);

                writer.WriteLine(username);
                writer.Flush();


                connected = true;
                return null;
            }
            catch (Exception ex)
            {
                return new Exception("Verbindung zu " + server + ":" + serverPort + " ist fehlgeschlagen");
            }
        }
    }
}

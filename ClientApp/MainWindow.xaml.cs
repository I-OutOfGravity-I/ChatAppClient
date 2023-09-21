using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClientApp
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;

    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private bool isConnected = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isConnected)
            {
                try
                {
                    string serverIp = ServerIpTextBox.Text;
                    int serverPort = 12345; // Ändere den Port entsprechend deines Servers

                    client = new TcpClient(serverIp, serverPort);
                    stream = client.GetStream();
                    reader = new StreamReader(stream, Encoding.UTF8);
                    writer = new StreamWriter(stream, Encoding.UTF8);

                    isConnected = true;
                    ConnectButton.Content = "Disconnect";
                    MessageTextBox.IsEnabled = true;
                    SendButton.IsEnabled = true;

                    // Starte einen Thread zum Empfangen von Nachrichten vom Server
                    Thread receiveThread = new Thread(ReceiveMessages);
                    receiveThread.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Verbindung fehlgeschlagen: " + ex.Message);
                }
            }
            else
            {
                Disconnect();
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                string message = MessageTextBox.Text;
                writer.WriteLine(message);
                writer.Flush();
                MessageTextBox.Clear();
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                while (isConnected)
                {
                    string message = reader.ReadLine();
                    if (message == null)
                    {
                        Disconnect();
                        break;
                    }

                    // Zeige die empfangene Nachricht in der TextBox an
                    Dispatcher.Invoke(() =>
                    {
                        ReceivedMessagesTextBox.AppendText(message + Environment.NewLine);
                    });
                }
            }
            catch (Exception)
            {
                // Verbindung wurde geschlossen
                Disconnect();
            }
        }

        private void Disconnect()
        {
            if (isConnected)
            {
                isConnected = false;
                ConnectButton.Content = "Connect";
                MessageTextBox.IsEnabled = false;
                SendButton.IsEnabled = false;

                writer.Close();
                reader.Close();
                stream.Close();
                client.Close();
            }
        }

        private void ConnectButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void SendButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }

}

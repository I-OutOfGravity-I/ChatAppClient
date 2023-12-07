using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;


namespace ClientApp
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private Controller c = Controller.Instance;

        public MainWindow()
        {
            InitializeComponent();
            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!c.Connected)
            {
                try
                {
                    string serverIp = "127.0.0.1";
                    int serverPort = 12345; // Ändere den Port entsprechend deines Servers

                    client = new TcpClient(serverIp, serverPort);
                    stream = client.GetStream();
                    reader = new StreamReader(stream, Encoding.UTF8);
                    writer = new StreamWriter(stream, Encoding.UTF8);

                    c.Connected = true;
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
            if (c.Connected)
            {
                string message = MessageTextBox.Text;
                c.Writer.WriteLine(message);
                c.Writer.Flush();
                MessageTextBox.Clear();
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                while (c.Connected)
                {
                    string message = c.Reader.ReadLine();
                    if (message == null)
                    {
                        Disconnect();
                        break;
                    }

                    // Zeige die empfangene Nachricht in der TextBox an
                    Dispatcher.Invoke(() =>
                    {
                        //ReceivedMessagesTextBox.AppendText(message + Environment.NewLine);
                        try
                        {


                            AddTextToRichTextBox(message, Colors.Red);
                        }
                        catch { }
                    });
                }
            }
            catch (Exception)
            {
                // Verbindung wurde geschlossen
                Disconnect();
            }
        }

        private void AddTextToRichTextBox(string text, Color color)
        {
            // Find the index of the first colon
            int colonIndex = text.IndexOf(':');

            // Create a new Run with the specified text and color
            Run run = new Run(text.Substring(0, colonIndex + 1)); // Include the colon in the red text
            run.Foreground = new SolidColorBrush(Colors.Blue);

            // Create a new Run for the rest of the text
            Run restRun = new Run(text.Substring(colonIndex + 1));
            restRun.Foreground = new SolidColorBrush(Colors.Black);

            // Create a new Paragraph and add both Runs
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(run);
            paragraph.Inlines.Add(restRun);

            // Set the Paragraph's margin to zero
            paragraph.Margin = new Thickness(0);

            // Get the existing content of the RichTextBox
            TextRange textRange = new TextRange(ReceivedMessagesTextBox.Document.ContentStart, ReceivedMessagesTextBox.Document.ContentEnd);

            // Add the new Paragraph to the Blocks collection
            ReceivedMessagesTextBox.Document.Blocks.Add(paragraph);

            // Set the RichTextBox padding to zero
            ReceivedMessagesTextBox.Padding = new Thickness(0);

            // Scroll to the end
            ReceivedMessagesTextBox.ScrollToEnd();
        }





        private void Disconnect()
        {
            if (c.Connected)
            {
                c.Connected = false;
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

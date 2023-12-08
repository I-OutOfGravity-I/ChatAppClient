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
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace ClientApp
{
    public enum MessageType
    {
        ServerNotification,
        Message,
        ConnectedUsers,
        Image,
        Video
    }
    public class Message
    {
        public MessageType Type { get; set; }
        public byte[] Content { get; set; }

        public Message(MessageType type, string content)
        {
            Type = type;
            Content = Encoding.UTF8.GetBytes(content);
        }

        public Message(MessageType type, List<string> content)
        {
            Type = type;
            Content = ConvertListToByteArray(content);
        }

        public Message(MessageType type, byte[] content)
        {
            Type = type;
            Content = content;
        }

        public string Serialize()
        {
            return $"{(int)Type}#{Convert.ToBase64String(Content)}";
        }

        public static T Deserialize<T>(string json)
        {
            T obj = (T)FormatterServices.GetUninitializedObject(typeof(T));

            // Remove curly braces and split by commas
            string[] keyValuePairs = json.Trim('{', '}').Split(',');

            foreach (var keyValuePair in keyValuePairs)
            {
                string[] keyValue = keyValuePair.Split(':');
                string propertyName = keyValue[0].Trim('\"');
                string propertyValue = keyValue[1].Trim('\"');

                var property = typeof(T).GetProperty(propertyName);

                if (property != null)
                {
                    if (property.PropertyType.IsEnum)
                    {
                        // Convert the string value to the enum type
                        object enumValue = MessageType.Parse(property.PropertyType, propertyValue);
                        property.SetValue(obj, enumValue);
                    }
                    else if (property.Name == "Content" && property.PropertyType == typeof(byte[]))
                    {
                        // Convert Base64 string to byte[] for JSON
                        byte[] contentBytes = Convert.FromBase64String(propertyValue);
                        property.SetValue(obj, contentBytes);
                    }
                }
            }
            
            return obj;
        }

        static byte[] ConvertListToByteArray(List<string> stringList)
        {
            // Use UTF-8 encoding to convert each string to bytes
            List<byte[]> stringBytesList = new List<byte[]>();
            foreach (string str in stringList)
            {
                byte[] strBytes = Encoding.UTF8.GetBytes(str);
                stringBytesList.Add(strBytes);
            }

            // Concatenate the byte arrays into a single byte array
            int totalLength = stringBytesList.Sum(arr => arr.Length);
            byte[] result = new byte[totalLength];
            int offset = 0;
            foreach (byte[] strBytes in stringBytesList)
            {
                Buffer.BlockCopy(strBytes, 0, result, offset, strBytes.Length);
                offset += strBytes.Length;
            }

            return result;
        }
    }
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private Controller c = Controller.Instance;


        public MainWindow()
        {
            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
            InitializeComponent();
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

                    //ReceivedMessagesTextBox.AppendText(message + Environment.NewLine);
                    Message receivedMessage = Message.Deserialize<Message>(message);

                    //Encoding.UTF8.GetString(receivedMessage.Content)
                    if (receivedMessage.Type == MessageType.ServerNotification) {
                        AddTextToRichTextBox(Encoding.UTF8.GetString(receivedMessage.Content), Colors.Red);
                    }
                    if (receivedMessage.Type == MessageType.Message)
                    {
                        AddTextToRichTextBox(Encoding.UTF8.GetString(receivedMessage.Content), Colors.Red); 
                    }
                    if (receivedMessage.Type == MessageType.ConnectedUsers)
                    {

                    }
                    if (receivedMessage.Type == MessageType.Image)
                    {

                    }
                    if (receivedMessage.Type == MessageType.Video)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                // Verbindung wurde geschlossen
                Disconnect();
            }
        }
        private void AddImageToRichTextBox(byte[] imageData)
        {
            // Create an Image control
            Image image = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageData);
            bitmapImage.EndInit();
            image.Source = bitmapImage;
            image.Width = 100;  // Adjust the width as needed
            image.Height = 100; // Adjust the height as needed

            // Create an InlineUIContainer to host the Image control
            InlineUIContainer inlineUIContainer = new InlineUIContainer(image);

            // Create a Paragraph and add the InlineUIContainer to it
            Paragraph paragraph = new Paragraph();
            paragraph.LineHeight = 1; // Adjust the line height to reduce spacing
            paragraph.Margin = new Thickness(0);

            paragraph.Inlines.Add(inlineUIContainer);

            // Add the Paragraph to the FlowDocument of the RichTextBox
            ReceivedMessagesTextBox.Document.Blocks.Add(paragraph);
        }

        private void AddTextToRichTextBox(string text, Color color)
        {
            Dispatcher.Invoke(() =>
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

                string imagePath = "C:\\Users\\outofgravity\\Desktop\\earth.jpg";
                byte[] imageData = File.ReadAllBytes(imagePath);
                AddImageToRichTextBox(imageData);
            });
        
        }


        private void Disconnect()
        {

        }
    }
}

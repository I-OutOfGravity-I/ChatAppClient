using ClientApp.Objects;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClientApp
{

    public partial class MainWindow : Window
    {
        private Controller c = Controller.Instance;
        private Thread receiveThread;
        private List<String> connectedUser;

        public MainWindow()
        {
            receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
            InitializeComponent();
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Disconnect();
            Environment.Exit(0);
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            sendMessage();
        }
        private void sendMessage()
        {
            if (c.Connected)
            {
                string message = MessageTextBox.Text;
                Message m = new Message(MessageType.Message, c.Username, MessageTextBox.Text);
                sendData(m);
            }
        }
        private void sendData(Message message)
        {
            string json = Message.SerializeMessage(message);
            c.Writer.WriteLine(json);
            c.Writer.Flush();
            MessageTextBox.Clear();
        }
        private void MessageTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sendMessage();
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

                    Message receivedMessage = Message.DeserializeMessage(message);

                    DisplayMessage(receivedMessage);
                }
            }
            catch (Exception ex)
            {
                Disconnect();
            }
        }
        private void DisplayMessage(Message message)
        {
            Dispatcher.Invoke(() =>
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Vertical;

                if (message.Type == MessageType.ConnectedUsers)
                {
                    refreshConnectedUser(Message.DeserializeStringList(message.Content));
                }
                else
                {
                    AddTextBlock(stackPanel, message);
                    AddImage(stackPanel, message);
                    AddVideo(stackPanel, message);

                    InlineUIContainer container = CreateContainer(stackPanel);
                    AddToRichTextBox(container);
                }
            });
        }

        private void AddTextBlock(StackPanel stackPanel, Message message)
        {
            if (message.Type == MessageType.Message || message.Type == MessageType.Image || message.Type == MessageType.Video)
            {
                string usernameBeginning = message.Username + ": ";
                Run usernameRun = new Run(usernameBeginning);
                usernameRun.Foreground = new SolidColorBrush(Colors.Blue);
                stackPanel.Children.Add(new TextBlock(usernameRun));
            }

            if (message.Type == MessageType.ServerNotification || message.Type == MessageType.Message)
            {
                Run contentRun = new Run(message.Content);
                contentRun.Foreground = new SolidColorBrush(Colors.Black);
                stackPanel.Children.Add(new TextBlock(contentRun));
            }
        }

        private void AddImage(StackPanel stackPanel, Message message)
        {
            if (message.Type == MessageType.Image)
            {
                System.Windows.Controls.Image image = CreateImage(message.Content);
                stackPanel.Children.Add(image);
            }
        }

        private void AddVideo(StackPanel stackPanel, Message message)
        {
            if (message.Type == MessageType.Video)
            {
                StackPanel mediaElement = CreateMediaElement(message.Content);
                stackPanel.Children.Add(mediaElement);
            }
        }

        private System.Windows.Controls.Image CreateImage(string base64Content)
        {
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(Convert.FromBase64String(base64Content));
            bitmapImage.EndInit();
            image.Source = bitmapImage;
            int desiredHeight = 200;
            ImageInfo imageInfo = ImageHelper.GetImageInfo(Convert.FromBase64String(base64Content), desiredHeight);
            image.Width = imageInfo.Width;
            image.Height = imageInfo.Height;
            return image;
        }

        private StackPanel CreateMediaElement(string base64Content)
        {
            MediaElement mediaElement = new MediaElement();
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Manual;
            mediaElement.Width = 300;
            mediaElement.Height = 169;
            SetMediaElementSource(mediaElement, Convert.FromBase64String(base64Content));
            StackPanel mediaControls = CreateMediaControls(mediaElement);
            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(mediaElement);
            stackPanel.Children.Add(mediaControls);
            return stackPanel;
        }

        private StackPanel CreateMediaControls(MediaElement mediaElement)
        {
            StackPanel mediaControls = new StackPanel();
            mediaControls.HorizontalAlignment = HorizontalAlignment.Center;
            mediaControls.Orientation = Orientation.Horizontal;

            Button playButton = CreateMediaButton("Play", () => mediaElement.Play());
            Button pauseButton = CreateMediaButton("Pause", () => mediaElement.Pause());
            Button muteButton = CreateMediaButton("Mute", () => mediaElement.IsMuted = !mediaElement.IsMuted);

            mediaControls.Children.Add(playButton);
            mediaControls.Children.Add(pauseButton);
            mediaControls.Children.Add(muteButton);

            return mediaControls;
        }

        private Button CreateMediaButton(string content, Action clickHandler)
        {
            Button button = new Button();
            button.Content = content;
            button.Click += (sender, e) => clickHandler.Invoke();
            return button;
        }

        private InlineUIContainer CreateContainer(Panel childPanel)
        {
            InlineUIContainer container = new InlineUIContainer();
            container.BaselineAlignment = BaselineAlignment.Top;

            Border border = new Border();
            border.CornerRadius = new CornerRadius(5);
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(1);
            border.Background = Brushes.LightGray;
            border.Child = childPanel;

            container.Child = border;
            return container;
        }

        private void AddToRichTextBox(InlineUIContainer container)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.LineHeight = 1;
            paragraph.Margin = new Thickness(2);
            paragraph.Inlines.Add(container);

            ReceivedMessagesTextBox.Document.Blocks.Add(paragraph);
            ReceivedMessagesTextBox.Padding = new Thickness(10);
            ReceivedMessagesTextBox.ScrollToEnd();
        }

        private void refreshConnectedUser(List<string> tempConnectedUser)
        {
            if (connectedUser == null || !tempConnectedUser.SequenceEqual(connectedUser))
            {
                connectedUser = tempConnectedUser;
                Dispatcher.Invoke(() =>
                {
                    connectedUserControl.Children.Clear();
                    foreach (var user in tempConnectedUser)
                    {
                        Button button = new Button();
                        button.Content = user;

                        connectedUserControl.Children.Add(button);
                    }
                });
            }
        }
        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Image",
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                byte[] imageData = File.ReadAllBytes(imagePath);

                string x = Convert.ToBase64String(imageData);
                sendData(new Message(MessageType.Image, c.Username, x));
            }
        }

        private void Disconnect()
        {
            c.Connected = false;

            // Close and dispose of resources
            c.Writer.Close();
            c.Reader.Close();
            c.Stream.Close();
            c.Client.Close();
        }

        private void SetMediaElementSource(MediaElement mediaElement, byte[] mediaData)
        {
            try
            {
                string tempFilePath = System.Environment.CurrentDirectory + "\\" + Random.Shared.NextInt64().ToString() + ".mp4";

                File.WriteAllBytes(tempFilePath, mediaData);

                mediaElement.Source = new Uri(tempFilePath, UriKind.Absolute);

                mediaElement.MediaEnded += (sender, e) => ((MediaElement)sender).Position = TimeSpan.Zero;
                mediaElement.MediaEnded += (sender, e) => ((MediaElement)sender).Pause();

                mediaElement.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private void SendVideo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Image",
                Filter = "Media files (*.mp4;*.avi;*.wmv;*.mpg;*.mpeg;*.mkv;*.mov)|*.mp4;*.avi;*.wmv;*.mpg;*.mpeg;*.mkv;*.mov|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string videoPath = openFileDialog.FileName;
                    byte[] videoData = File.ReadAllBytes(videoPath);

                    string x = Convert.ToBase64String(videoData);
                    sendData(new Message(MessageType.Video, c.Username, x));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading video file: {ex.Message}");
                }
            }
        }
    }
}

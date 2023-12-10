using ClientApp.Objects;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
        }
        public event EventHandler<Tuple<string, MessageType>> SendMessage;
        public event EventHandler WindowClose;

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            WindowClose?.Invoke(sender, e);
            Environment.Exit(0);
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage?.Invoke(this, Tuple.Create(MessageTextBox.Text, MessageType.Message));
        }
        private void MessageTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage?.Invoke(this, Tuple.Create(MessageTextBox.Text, MessageType.Message));
            }
        }
        public void ClearMessageBox()
        {
            MessageTextBox.Clear();
        }

        #region Chat Fenster Medien hinzufügen
        public void DisplayMessage(Message message)
        {
            Dispatcher.Invoke(() =>
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Vertical;
                AddTextBlock(stackPanel, message);
                AddImage(stackPanel, message);
                AddVideo(stackPanel, message);

                InlineUIContainer container = CreateContainer(stackPanel);
                AddToRichTextBox(container);
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
        private Image CreateImage(string base64Content)
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
        #endregion

        public void RefreshUserList(List<string> users)
        {
            Dispatcher.Invoke(() =>
            {
                connectedUserControl.Children.Clear();
                foreach (var user in users)
                {
                    Button button = new Button();
                    button.Content = user;

                    connectedUserControl.Children.Add(button);
                }
            });
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
                string imageStringData = Convert.ToBase64String(imageData);

                SendMessage?.Invoke(this, Tuple.Create(imageStringData, MessageType.Image));
            }
        }
        private void SendVideo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Video",
                Filter = "Media files (*.mp4;*.avi;*.wmv;*.mpg;*.mpeg;*.mkv;*.mov)|*.mp4;*.avi;*.wmv;*.mpg;*.mpeg;*.mkv;*.mov|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string videoPath = openFileDialog.FileName;
                    byte[] videoData = File.ReadAllBytes(videoPath);

                    string videoDataString = Convert.ToBase64String(videoData);
                    SendMessage?.Invoke(this, Tuple.Create(videoDataString, MessageType.Video));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading video file: {ex.Message}");
                }
            }
        }
    }
}

namespace ClientApp
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    public partial class Login : Window
    {
        private bool serverAlreadyEmptied = false;
        private bool UsernameAlreadyEmptied = false;

        public event EventHandler<Tuple<string, string>> LoginEvent;

        public Login()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginEvent?.Invoke(this, Tuple.Create(Server.Text, Username.Text));
        }

        private void Top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
        private void Server_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (serverAlreadyEmptied == false)
            {
                Server.Text = string.Empty;
                Server.Foreground = new SolidColorBrush(Colors.Black);
                serverAlreadyEmptied = true;
            }
        }
        private void Server_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Server.Text == "")
            {
                Server.Text = "Server Name/IP";
                Server.Foreground = new SolidColorBrush(Colors.Gray);
                serverAlreadyEmptied = false;
            }
        }
        private void Username_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (UsernameAlreadyEmptied == false)
            {
                Username.Text = string.Empty;
                Username.Foreground = new SolidColorBrush(Colors.Black);
                UsernameAlreadyEmptied = true;
            }
        }

        private void Username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "")
            {
                Username.Text = "Benutzername";
                Username.Foreground = new SolidColorBrush(Colors.Gray);
                UsernameAlreadyEmptied = false;
            }
        }

        public void PlayLoading()
        {
            this.Dispatcher.Invoke(new Action(() => {
                loading.Visibility = Visibility.Visible;
            }));
        }
        internal void SetFehlerTextEmpty()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                fehler.Text = "";
            }));
        }

        internal void DisplayLoginError(Exception ex)
        {
            this.Dispatcher.Invoke(new Action(() => {
                loading.Visibility = Visibility.Hidden;
                fehler.Text = ex.Message;
            }));
        }

        internal void CloseWindow()
        {
            this.Dispatcher.Invoke(new Action(() => {
                this.Close();
            }));
        }
    }
}

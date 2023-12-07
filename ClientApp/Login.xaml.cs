﻿using System;
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
    using System.Reflection.PortableExecutable;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public partial class Login : Window
    {
        private bool serverAlreadyEmptied = false;
        private bool UsernameAlreadyEmptied = false;
        private Controller c = Controller.Instance;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string server = Server.Text;
            string username = Username.Text;

            fehler.Text = "";

            Thread t = new Thread(playLoading);
            t.Start();


            Thread x = new Thread(() => tryLoggingIn(server, username));
            x.Start();
        }

        private void tryLoggingIn(string server, string username)
        {
            Thread.Sleep(800);

            Exception ex = c.tryConnection(server, username);
            if (ex != null)
            {
                this.Dispatcher.Invoke(new Action(() => {
                    loading.Visibility = Visibility.Hidden;
                    fehler.Text = ex.Message;
                }));
            }
            else
            {
                Thread x = new Thread(() => openWindow());
                x.SetApartmentState(ApartmentState.STA);
                x.Start();
                this.Dispatcher.Invoke(new Action(() => {
                    this.Close();
                }));
            }
        }
        private void openWindow()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
        private void playLoading()
        {
            this.Dispatcher.Invoke(new Action(() => {
                loading.Visibility = Visibility.Visible;
            }));
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
    }
}
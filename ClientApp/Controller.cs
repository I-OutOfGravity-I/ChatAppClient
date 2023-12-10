using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using ClientApp.Objects;

namespace ClientApp
{
    public class Controller
    {
        private Model _model = new Model();
        private Login _loginView;
        private MainWindow _mainView;

        private Thread receiveThread;

        public Controller(Login login)
        {
            _loginView = login;
            _loginView.LoginEvent += LoginFromEvent;
        }
        private void OpenWindow()
        {
            _mainView = new MainWindow();
            _mainView.WindowClose += DisconnectFromEvent;
            _mainView.SendMessage += SendMessageFromEvent;
            StartReceiveMessageThread();
            _mainView.ShowDialog();
        }
        private void SendMessageFromEvent(object? sender, Tuple<string, MessageType> parameters)
        {
            SendMessage(parameters.Item1, parameters.Item2);
        }

        private void DisconnectFromEvent(object? sender, EventArgs e)
        {
            Disconnect();
        }
        private void Disconnect()
        {
            _model.Connected = false;

            _model.Writer.Close();
            _model.Reader.Close();
            _model.Stream.Close();
            _model.Client.Close();
        }

        private void LoginFromEvent(object sender, Tuple<string, string> parameters)
        {
            string server = parameters.Item1;
            string username = parameters.Item2;

            _loginView.SetFehlerTextEmpty();

            Thread t = new Thread(_loginView.PlayLoading);
            t.Start();


            Thread x = new Thread(() => TryLoggingIn(server, username));
            x.Start();
        }
        private void TryLoggingIn(string server, string username)
        {
            Thread.Sleep(800);

            Exception ex = TryConnectionAndLogin(server, username);
            if (ex != null)
            {
                _loginView.DisplayLoginError(ex);
            }
            else
            {
                Thread x = new Thread(() => OpenWindow());
                x.SetApartmentState(ApartmentState.STA);
                x.Start();
                Thread.Sleep(50);
                _loginView.CloseWindow();
            }
        }

        public Exception? TryConnectionAndLogin(string server, string username)
        {
            _model.Username = username;
            string serverIp = server;
            int serverPort = 12345;
            try
            { 
                _model.Client = new TcpClient(serverIp, serverPort);
                _model.Stream = _model.Client.GetStream();
                _model.Reader = new StreamReader(_model.Stream, Encoding.UTF8);
                _model.Writer = new StreamWriter(_model.Stream, Encoding.UTF8);

                _model.Connected = true;

                SendMessage("", MessageType.Login);

                return null;
            }
            catch
            {
                return new Exception("Verbindung zu " + server + ":" + serverPort + " ist fehlgeschlagen");
            }
        }
        public void SendMessage(string message, MessageType messageType)
        {
            if (_model.Connected)
            {
                Message m = new Message(messageType, _model.Username, message);
                string json = Message.SerializeMessage(m);
                _model.Writer.WriteLine(json);
                _model.Writer.Flush();
                if (_mainView != null)
                {
                    _mainView.ClearMessageBox();
                }
            }
        }
        private void ReceiveMessages()
        {
            try
            {
                while (_model.Connected)
                {
                    string message = _model.Reader.ReadLine();
                    if (message == null)
                    {
                        Disconnect();
                        break;
                    }

                    Message receivedMessage = Message.DeserializeMessage(message);

                    if (receivedMessage.Type == MessageType.ConnectedUsers)
                    {
                        RefreshConnectedUser(Message.DeserializeStringList(receivedMessage.Content));
                    }
                    else
                    {
                        _mainView.DisplayMessage(receivedMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Disconnect();
            }
        }
        private void RefreshConnectedUser(List<string> tempConnectedUser)
        {
            if (_model.ConnectedUser == null || !tempConnectedUser.SequenceEqual(_model.ConnectedUser))
            {
                _model.ConnectedUser = tempConnectedUser;
                _mainView.RefreshUserList(_model.ConnectedUser);
            }
        }
        internal void StartReceiveMessageThread()
        {
            receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }
    }
}

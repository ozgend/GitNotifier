using denolk.GitNotifierClient.Model;
using GitNotifierClient.Helper;
using GitNotifierClient.View;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GitNotifierClient
{

    public partial class MainWindow : Window
    {

        private ObservableCollection<string> _listSource = new ObservableCollection<string>();
        private HubConnection _connection;
        private string _serviceUrl = Config.GetServiceLocation();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Panel_Loaded(object sender, RoutedEventArgs e)
        {
            listNotifications.ItemsSource = _listSource;
            ConnectToHub();
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkAvailabilityChanged);
        }

        private void ConnectToHub()
        {
            Task.Factory.StartNew(() => _ConnectToHub());
        }

        private void _ConnectToHub()
        {
            if (_connection != null)
            {
                _connection.Disconnect();
                _connection = null;
            }
            _connection = new HubConnection(_serviceUrl);
            _connection.Credentials = CredentialCache.DefaultNetworkCredentials;
            _connection.StateChanged += new Action<Microsoft.AspNet.SignalR.Client.StateChange>(HubConnectionStateChanged);
            IHubProxy chat = _connection.CreateHubProxy("ClientNotificationHub");
            chat.On<Message>("Send", MessageReceived);
            _connection.Start().Wait();
        }

        private void NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                ConnectToHub();
            }
            else
            {
                var message = new Message { Text = "Connection lost with service", Url = _serviceUrl, IsLocal = true };
                CreateNotificationFromMainDispatcher(message);
            }
        }

        private void HubConnectionStateChanged(StateChange state)
        {
            Debug.WriteLine("HubConnectionStateChanged Old={0}  New={1}", state.OldState, state.NewState);

            if (state.NewState == ConnectionState.Connected)
            {
                var message = new Message { Text = "Connected to service", Url = _serviceUrl, IsLocal = true };
                CreateNotificationFromMainDispatcher(message);
            }
        }

        private void MessageReceived(Message message)
        {
            try
            {
                CreateNotificationFromDispatcher(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Received= " + message.ToString());
                Debug.WriteLine(ex.ToString());
            }
        }

        private void CreateNotificationFromMainDispatcher(Message message)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate()
            {
                _CreateNotification(message);
            });
        }

        private void CreateNotificationFromDispatcher(Message message)
        {
            listNotifications.Dispatcher.BeginInvoke(new Action(delegate()
            {
                _CreateNotification(message);
            }));
        }

        private void _CreateNotification(Message message)
        {
            _listSource.Insert(0, message.ToString());

            BalloonBox balloon = new BalloonBox();
            balloon.BalloonText = "GitHub Notifier";
            balloon.BalloonMessage = message.ToString();
            if (!string.IsNullOrEmpty(message.Url) && !message.IsLocal)
            {
                balloon.AdditionalClickEvent = new Task(() => Process.Start(message.Url));
            }
            DNotificationIcon.ShowCustomBalloon(balloon, PopupAnimation.Slide, 6000);

        }



    }
}

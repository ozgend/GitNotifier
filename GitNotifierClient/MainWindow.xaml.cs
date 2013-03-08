using denolk.GitNotifierClient.Model;
using GitNotifierClient.Helper;
using GitNotifierClient.View;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

namespace GitNotifierClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ObservableCollection<string> listSource = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Panel_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void Start()
        {
            listNotifications.ItemsSource = listSource;
            Task.Factory.StartNew(() => Connect());
        }

        private void Connect()
        {
            string serviceUrl = Config.GetServiceLocation();
            var connection = new HubConnection(serviceUrl);
            connection.Credentials = CredentialCache.DefaultNetworkCredentials;
            IHubProxy chat = connection.CreateHubProxy("ClientNotificationHub");
            chat.On<Message>("Send", MessageReceived);
            connection.Start().Wait();
        }


        private void MessageReceived(Message message)
        {
            try
            {
                listNotifications.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    CreateNotification(message);
                }));

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Received= " + message.ToString());
                Debug.WriteLine(ex.ToString());
            }
        }

        private void CreateNotification(Message message)
        {
            listSource.Insert(0, message.ToString());

            BalloonBox balloon = new BalloonBox();
            balloon.BalloonText = "GitHub Notification";
            balloon.BalloonMessage = message.ToString();
            balloon.AdditionalClickEvent = new Task(() => Process.Start(message.Url));

            DNotificationIcon.ShowCustomBalloon(balloon, PopupAnimation.Slide, 6000);

        }



    }
}

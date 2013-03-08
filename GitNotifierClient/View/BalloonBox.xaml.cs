using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GitNotifierClient.View
{
    public partial class BalloonBox : UserControl
    {

        public BalloonBox()
        {
            InitializeComponent();
            TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);
        }

        private bool isClosing = false;
        public Task AdditionalClickEvent = null;

        public static readonly DependencyProperty BalloonTextProperty =
            DependencyProperty.Register("BalloonText",
                                        typeof(string),
                                        typeof(BalloonBox),
                                        new FrameworkPropertyMetadata(""));

        public static readonly DependencyProperty BalloonMessageProperty =
            DependencyProperty.Register("BalloonMessage",
                                        typeof(string),
                                        typeof(BalloonBox),
                                        new FrameworkPropertyMetadata(""));

        public string BalloonText
        {
            get { return (string)GetValue(BalloonTextProperty); }
            set { SetValue(BalloonTextProperty, value); }
        }

        public string BalloonMessage
        {
            get { return (string)GetValue(BalloonMessageProperty); }
            set { SetValue(BalloonMessageProperty, value); }
        }

        
        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            isClosing = true;
        }

        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (AdditionalClickEvent != null)
            {
                AdditionalClickEvent.Start();
            }
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();            
        }

        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isClosing) return;

            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.ResetBalloonCloseTimer();
        }
        
        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            Popup pp = (Popup)Parent;
            pp.IsOpen = false;
        }

    }
}

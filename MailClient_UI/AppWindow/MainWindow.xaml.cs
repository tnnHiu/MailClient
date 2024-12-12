
using MailClient_Controller.Enities;
using MailClient_Controller.MailController;
using MailClient_Controller.Service;
using MailClient_UI.AppWindow.MailControl;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace MailClient_UI
{
    public partial class MainWindow : Window
    {
        MailController mailControler;
        List<Mail> mailList;
        public string Username { get; set; }

        public MainWindow(string username)
        {
            InitializeComponent();
            Username = username;
            LoadUserControl(new InboxUC(Username)); 
        }
        private bool IsMaximize = false;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnCompose_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new ComposeUC(Username));
        }

        private void btnInbox_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new InboxUC(Username));
        }

        private void btnStarred_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSent_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new SentUC(Username));
        }

        private void btnTrash_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new TrashUC(Username));
        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new AllUC(Username));
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadUserControl(UserControl userControl)
        {
            if (userControl != null)
            {
                this.userControl.Content = userControl;
            }
        }
    }
}
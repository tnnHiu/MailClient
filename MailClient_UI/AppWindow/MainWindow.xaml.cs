
using MailClient_Controller.Enities;
using MailClient_Controller.MailController;
using MailClient_Controller.Service;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

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
            if(!IMAPService.Instance._client.Connected)
            {
                Debug.WriteLine("Client not connected");
            }
            fetchMailBox();
        }
        private void fetchMailBox()
        {
            mailControler = new MailController(Username);
            mailList = mailControler.fetchMail();
            membersDataGrid.ItemsSource = mailList;
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

        private void Compose_Button(object sender, RoutedEventArgs e)
        {

        }

        private void AllMail_Button(object sender, RoutedEventArgs e)
        {

        }

        private void Trash_Button(object sender, RoutedEventArgs e)
        {

        }

        private void Sent_Button(object sender, RoutedEventArgs e)
        {

        }

        private void Inbox_Button(object sender, RoutedEventArgs e)
        {

        }

        private void SignOut_Button(object sender, RoutedEventArgs e)
        {

        }

        private void btnComposeMail_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
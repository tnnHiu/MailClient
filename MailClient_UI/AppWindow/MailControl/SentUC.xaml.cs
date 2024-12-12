using MailClient_Controller.Enities;
using MailClient_Controller.MailController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MailClient_UI.AppWindow.MailControl
{
    /// <summary>
    /// Interaction logic for SentUC.xaml
    /// </summary>
    public partial class SentUC : UserControl
    {

        private MailController mailControler;
        private List<Mail> mailList;
        public string Username { get; set; }
        public SentUC(string username)
        {
            InitializeComponent();
            Username = username;
            fetchMailBox();
        }

        private void fetchMailBox()
        {
            mailControler = new MailController(Username);
            mailList = mailControler.fetchSentMail();
            mailsDataGrid.ItemsSource = mailList;
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            fetchMailBox();
        }
    }
}

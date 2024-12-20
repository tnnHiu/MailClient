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
using System.Windows.Shapes;

namespace MailClient_UI.AppWindow.Modal
{
    /// <summary>
    /// Interaction logic for ForwardModal.xaml
    /// </summary>
    public partial class ForwardModal : Window
    {

        MailController mailController;

        public Mail MailForward { get; set; }
        public string UserName { get; set; }

        public ForwardModal(Mail mail, string username)
        {
            InitializeComponent();
            MailForward = mail;
            UserName = username;
            ShowMailForwardProperty();
        }

        private void ShowMailForwardProperty()
        {
            txtContent.Text = MailForward.Content;
            txtSubject.Text = MailForward.Subject;

        }


        private void btnSendForward_Click(object sender, RoutedEventArgs e)
        {
            int id = MailForward.Id;
            string userEmail = UserName + "@vku.udn.vn";
            string receiver = txtTo.Text;

            Mail mailForward = new Mail(id, userEmail, receiver);
            mailController = new MailController(UserName);
            if (mailController.ForwardEmail(mailForward))
            {
                CloseModal();
            }
            else
            {
                MessageBox.Show("Err");
            }
        }

        private void btnAttach_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CloseModal();
        }

        private void CloseModal()
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}

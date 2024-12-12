using MailClient_Controller.Enities;
using MailClient_Controller.MailController;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ComposeModal.xaml
    /// </summary>
    public partial class ComposeModal : Window
    {

        MailController mailController;
        public string Username { get; set; }
        public ComposeModal(string username)
        {
            InitializeComponent();
            Username = username;
        }

        private void CloseModal()
        {
            this.DialogResult = true;
            this.Close();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CloseModal();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string username = Username + "@vku.udn.vn";
            string receiver = txtTo.Text;
            string subject = txtSubject.Text;
            string content = txtContent.Text;
            Mail mail = new Mail(username, receiver, subject, content);
            mailController = new MailController(Username);
            if (mailController.SendEmail(mail))
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
    }
}

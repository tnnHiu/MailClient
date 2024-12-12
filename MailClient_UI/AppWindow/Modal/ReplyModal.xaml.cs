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
    /// Interaction logic for ReplyModal.xaml
    /// </summary>
    public partial class ReplyModal : Window
    {

        MailController mailController;

        public Mail MailReply { get; set; }
        public string UserName { get; set; }

        public ReplyModal(Mail mail, string username)
        {
            InitializeComponent();
            MailReply = mail;
            UserName = username;
            ShowMailReplyProperty();
        }
        private void ShowMailReplyProperty()
        {
            txtTo.Text = MailReply.Sender;
        }

        private void btnSendReply_Click(object sender, RoutedEventArgs e)
        {
            int id = MailReply.Id;
            string userEmail = UserName + "@vku.udn.vn";
            string receiver = txtTo.Text;
            string subject = txtSubject.Text;
            string content = txtContent.Text;
            Mail mailReply = new Mail(id, userEmail, receiver, subject, content);
            mailController = new MailController(UserName);
            if (mailController.ReplyEmail(mailReply))
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

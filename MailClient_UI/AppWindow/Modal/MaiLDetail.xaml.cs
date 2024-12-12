
using MailClient_Controller.Enities;
using MailClient_Controller.MailController;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace MailClient_UI.AppWindow.Modal
{

    public partial class MaiLDetail : Window
    {
        public List<Mail> mails = new List<Mail>();
        MailController mailController;
        public int MailId { get; set; }
        public string UserName { get; set; }


        public MaiLDetail(int mailId, string username )
        {
            InitializeComponent();

            MailId = mailId;
            UserName = username;

            if (UserName != null)
            {
                mailController = new MailController(UserName);
            }
            mails = GetMail(MailId);
            ShowMail();
        }

        private List<Mail> GetMail(int id)
        {
            return mailController.fetchMailDetail(id);
        }

        private void ShowMail()
        {
            Mail mail = mails.First();

            if (mail != null)
            {
                txtFrom.Text = mail.Sender;
                txtTo.Text = mail.Receiver;
                txtSubject.Text = mail.Subject;
                txtContent.Text = mail.Content;
                if (mail.Attachment == null)
                {
                    attachRow.Visibility = Visibility.Hidden;
                }
                else
                {
                    txtAttach.Text = mail.Attachment;
                }
            }
            else
            {
                MessageBox.Show("Fail to load mail");
            }
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

        private void btnReply_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

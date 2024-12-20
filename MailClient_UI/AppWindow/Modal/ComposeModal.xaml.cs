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
        private string filePath;
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


            if (txtAttach.Text == "")
            {

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
            else
            {
                string attachemnt = txtAttach.Text;
                Mail mail = new Mail(username, receiver, subject, content, attachemnt);
                mailController = new MailController(Username, filePath);
                if (mailController.SendMailWithAttach(mail))
                {
                    if (mailController.SendFile(mail))
                    {
                        CloseModal();
                    }
                    else
                    {
                        MessageBox.Show("Err null iden");
                    }
                }
                else
                {
                    MessageBox.Show("Err");
                }
            }
        }
        private void btnAttach_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result != null)
            {
                attachRow.Visibility = Visibility.Visible;
                filePath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(filePath);
                txtAttach.Text = fileName;
            }
        }
    }
}

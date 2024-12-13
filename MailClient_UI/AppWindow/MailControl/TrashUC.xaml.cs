using MailClient_Controller.Enities;
using MailClient_Controller.MailController;
using MailClient_UI.AppWindow.Modal;
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
    /// Interaction logic for TrashUC.xaml
    /// </summary>
    public partial class TrashUC : UserControl
    {
        private MailController mailControler;
        private List<Mail> mailList;
        public string Username { get; set; }
        public TrashUC(string username)
        {
            InitializeComponent();
            Username = username;
            fetchMailBox();
        }

        private void fetchMailBox()
        {
            mailControler = new MailController(Username);
            mailList = mailControler.fetchtTrashMail();
            mailsDataGrid.ItemsSource = mailList;
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            fetchMailBox();
        }

        private void btnMoveToTrash_Click(object sender, RoutedEventArgs e)
        {

            // Lấy Button đã được bấm
            var button = sender as Button;

            // Lấy Id từ CommandParameter
            if (button?.CommandParameter is int mailId)
            {
                // Xác nhận xóa mail
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa mail với mailId {mailId} này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Thực hiện logic xóa mail
                    mailControler.moveToTrash(mailId);

                    // Refresh lại danh sách
                    fetchMailBox();
                }
                else
                {
                    // Hủy sự kiện nếu người dùng chọn "No"
                    return;
                }
            }
            else
            {
                MessageBox.Show("Không lấy được Id của mail.");
            }       

        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            // Lấy Button đã được bấm
            var button = sender as Button;

            // Lấy Id từ CommandParameter
            if (button?.CommandParameter is int mailId)
            {

                // Thực hiện logic khôi phục mail
                mailControler.restoreMail(mailId);

                // Refresh lại danh sách
                fetchMailBox();
            }
            else
            {
                MessageBox.Show("Không lấy được Id của mail.");
            }
        }

        private void mailsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mail? mailSelected = mailsDataGrid.SelectedItem as Mail;
            if (mailSelected != null)
            {
                MaiLDetail mailDetail = new MaiLDetail(mailSelected.Id, Username)
                {
                    Owner = Window.GetWindow(this),
                };
                mailDetail.ShowDialog();
            }
        }
    }
}


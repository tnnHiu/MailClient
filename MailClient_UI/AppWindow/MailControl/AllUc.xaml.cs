using MailClient_Controller.Enities;
using MailClient_Controller.MailController;
using MailClient_UI.AppWindow.Modal;
using System.Windows;
using System.Windows.Controls;


namespace MailClient_UI.AppWindow.MailControl
{
    /// <summary>
    /// Interaction logic for AllUc.xaml
    /// </summary>
    public partial class AllUC : UserControl
    {
        private MailController mailControler;
        private List<Mail> mailList;
        public string Username { get; set; }
        public AllUC(string username)
        {
            InitializeComponent();
            Username = username;
            fetchMailBox();
        }

        private void fetchMailBox()
        {
            mailControler = new MailController(Username);
            mailList = mailControler.fetchAllMail();
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

                // xác nhận xóa mail
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa mail với mailId {mailId} này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);


                // Thực hiện logic xóa mail
                mailControler.moveToTrash(mailId);

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
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
        public ForwardModal()
        {
            InitializeComponent();
        }

        private void btnSendForward_Click(object sender, RoutedEventArgs e)
        {

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

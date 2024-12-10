using MailClient_Controller.Auth_Controller;
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

namespace MailClient_UI.AuthWindow
{
    /// <summary>
    /// Interaction logic for SignInUC.xaml
    /// </summary>
    public partial class SignInUC : UserControl
    {
        AuthController authController = new AuthController();
        public SignInUC()
        {
            InitializeComponent();
        }

        private void btnToSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUpUC signUpUC = new SignUpUC();
            AuthWindow authWindow = (AuthWindow)Application.Current.MainWindow;
            authWindow.mainContentControl.Content = signUpUC;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (authController.SignUp())
            {
                MessageBox.Show("OK");
            }
            else
            {
                MessageBox.Show("Err");
            }
        }
    }
}

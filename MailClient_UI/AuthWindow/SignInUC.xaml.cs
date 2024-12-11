using MailClient_Controller.Auth_Controller;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
            }
            else
            {
                textEmail.Visibility = Visibility.Visible;
            }
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }
        private void btnToSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUpUC signUpUC = new SignUpUC();
            AuthWindow authWindow = (AuthWindow)Application.Current.MainWindow;
            authWindow.mainContentControl.Content = signUpUC;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = txtEmail.Text;
            string password = txtPassword.Password;

            if (authController.SignIn(username, password))
            {
                try
                {
                    MainWindow mainWindow = new MainWindow(username);
                    mainWindow.Show();
                    Window thisWindow = Window.GetWindow(this);
                    thisWindow.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"BtnSignIn {ex}");
                }
            }
            else
            {
                MessageBox.Show("Err");
            }
        }
    }
}

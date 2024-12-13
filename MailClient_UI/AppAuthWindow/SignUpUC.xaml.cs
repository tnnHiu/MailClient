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

namespace MailClient_UI.AppAuthWindow
{
    /// <summary>
    /// Interaction logic for SignUpUC.xaml
    /// </summary>
    public partial class SignUpUC : UserControl
    {
        AuthController authController = new AuthController();

        public SignUpUC()
        {
            InitializeComponent();
        }

        private void textName_MouseDown(object sender, MouseEventArgs e)
        {
            txtName.Focus();
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text) && txtName.Text.Length > 0)
            {
                textName.Visibility = Visibility.Collapsed;
            }
            else
            {
                textName.Visibility = Visibility.Visible;
            }
        }


        private void textEmail_MouseDown(object sender, MouseEventArgs e)
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

        private void textConfirmPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtConfirmPassword.Focus();
        }

        private void txtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtConfirmPassword.Password) && txtConfirmPassword.Password.Length > 0)
            {
                textConfirmPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textConfirmPassword.Visibility = Visibility.Visible;
            }
        }

        private void btnToSignIn_Click(object sender, RoutedEventArgs e)
        {
            //SignInUC signInUC = new SignInUC();
            //AuthWindow authWindow = (AuthWindow)Application.Current.MainWindow;
            //authWindow.mainContentControl.Content = signInUC;
            SignInUC signInUC = new SignInUC();
            AuthWindow authWindow = Window.GetWindow(this) as AuthWindow;
            if (authWindow != null)
            {
                authWindow.mainContentControl.Content = signInUC;
            }
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string username = txtEmail.Text;
            string fullname = txtName.Text;
            string password = txtPassword.Password;
            if (authController.SignUp(username, fullname, password))
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
                //SignInUC signInUC = new SignInUC();
                ////AuthWindow authWindow = (AuthWindow)Application.Current.MainWindow;
                ////authWindow.mainContentControl.Content = signInUC;
            }
            else
            {
                MessageBox.Show("Err");
            }
        }
    }
}

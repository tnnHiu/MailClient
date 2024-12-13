
using System.Windows;


namespace MailClient_UI.AppAuthWindow
{

    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
            SignUpUC signUpUC = new SignUpUC();
            mainContentControl.Content = signUpUC;
        }
    }
}

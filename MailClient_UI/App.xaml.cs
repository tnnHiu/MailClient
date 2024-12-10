using MailClient_Controller.Service;
using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;

namespace MailClient_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                IMAPService.Instance.Initialize("localhost");
                IMAPService.Instance.StartService();

                SMTPService.Instance.Initialize("localhost");
                SMTPService.Instance.StartService();

                FTPService.Instacnce.Initialize("localhost");
                FTPService.Instacnce.StartService();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnStartUp: {ex.Message}");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            try
            {
                IMAPService.Instance.StopService();
                SMTPService.Instance.StopService();
                FTPService.Instacnce.StopService();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnExit: {ex.Message}");
            }
        }
    }
}

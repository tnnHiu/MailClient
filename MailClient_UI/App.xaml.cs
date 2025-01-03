﻿using MailClient_Controller.Auth_Controller;
using MailClient_Controller.Service;
using System.Windows;

namespace MailClient_UI
{

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                //string serverIp = "192.168.1.16";
                string serverIp = "192.168.239.190";
                IMAPService.Instance.Initialize(serverIp);
                SMTPService.Instance.Initialize(serverIp);
                FTPService.Instance.Initialize(serverIp);
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
                FTPService.Instance.StopService();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnExit: {ex.Message}");
            }
        }
    }
}

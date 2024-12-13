using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MailClient_Controller.Service
{
    public class FTPService
    {
        private static readonly Lazy<FTPService> _instance = new(() => new FTPService());
        public TcpClient _client;
        private const int FtpPort = 21;

        public string ServerIp { get; set; } = string.Empty;

        private FTPService() { }

        // Singleton Instance property.
        public static FTPService Instance => _instance.Value;

        public void Initialize(string serverIp)
        {
            if (string.IsNullOrWhiteSpace(serverIp))
                throw new ArgumentException("Server IP cannot be null or empty.");
            ServerIp = serverIp;
            try
            {
                _client = new TcpClient(ServerIp, FtpPort);
                Debug.WriteLine("Connected to FTP server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Initialize: {ex.Message}");
            }
        }

        public void StopService()
        {
            try
            {
                if (_client != null && _client.Connected)
                {
                    _client.Close();
                    Debug.WriteLine("FTP Service stopped.");
                }
                else
                {
                    Debug.WriteLine("FTP Service is not running.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while stopping FTP service: {ex.Message}");
            }
        }


    }
}

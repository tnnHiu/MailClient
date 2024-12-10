using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MailClient_Controller.Service
{
    public class FTPService
    {
        private static readonly Lazy<FTPService> _instance = new(() => new FTPService());
        public TcpClient? _client;
        private const int FtpPort = 21;

        public string ServerIp { get; set; } = string.Empty;

        private FTPService() { }

        // Singleton Instance property.
        public static FTPService Instacnce => _instance.Value;

        public void Initialize(string serverIp)
        {
            if (string.IsNullOrWhiteSpace(serverIp))
            {
                throw new ArgumentException("Server IP cannot be null or empty.", nameof(serverIp));
            }

            ServerIp = serverIp;
        }

        public void StartService()
        {
            if (string.IsNullOrWhiteSpace(ServerIp))
            {
                Console.WriteLine("Cannot start FTP service. Server IP is not initialized.");
                return;
            }
            try
            {
                if (_client == null)
                {
                    _client = new TcpClient(ServerIp, FtpPort);
                    Console.WriteLine("IMAP Connected.");
                }
                else if (!_client.Connected)
                {
                    // Nếu _client đã được khởi tạo nhưng không còn kết nối, tạo lại kết nối
                    _client.Close();
                    _client = new TcpClient(ServerIp, FtpPort);
                    Console.WriteLine("IMAP Reconnected.");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Failed to connect to FTP server: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while starting FTP service: {ex}");
            }
        }

        public void StopService()
        {
            try
            {
                if (_client != null && _client.Connected)
                {
                    _client.Close();
                    Console.WriteLine("FTP Service stopped.");
                }
                else
                {
                    Console.WriteLine("FTP Service is not running.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while stopping FTP service: {ex.Message}");
            }
        }


    }
}


using System.Net.Sockets;

namespace MailClient_Controller.Service
{
    public class SMTPService
    {

        private static readonly Lazy<SMTPService> _instance = new(() => new SMTPService());
        public TcpClient _client;
        private const int SMTPPort = 25;

        public string ServerIp { get; private set; } = string.Empty;

        private SMTPService() { }

        // Singleton Instance property.
        public static SMTPService Instance => _instance.Value;

        public void Initialize(string serverIp)
        {
            if (string.IsNullOrWhiteSpace(serverIp))
                throw new ArgumentException("Server IP cannot be null or empty.", nameof(serverIp));

            ServerIp = serverIp;
        }

        public void StartService()
        {
            if (string.IsNullOrWhiteSpace(ServerIp))
            {
                Console.WriteLine("Cannot start SMTP service. Server IP is not initialized.");
                return;
            }

            try
            {
                if (_client == null)
                {
                    _client = new TcpClient(ServerIp, SMTPPort);
                    Console.WriteLine("SMTP Connected.");
                }
                else if (!_client.Connected)
                {
                    _client.Close();
                    _client = new TcpClient(ServerIp, SMTPPort);
                    Console.WriteLine("IMAP Reconnected.");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Failed to connect to SMTP server: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while starting SMTP service: {ex}");
            }
        }

        public void StopService()
        {
            try
            {
                if (_client != null && _client.Connected)
                {
                    _client.Close();
                    Console.WriteLine("SMTP Service stopped.");
                }
                else
                {
                    Console.WriteLine("SMTP Service is not running.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while stopping SMTP service: {ex.Message}");
            }
        }
    }
}

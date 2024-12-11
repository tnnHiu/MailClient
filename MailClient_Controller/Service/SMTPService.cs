
using System.Diagnostics;
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
                throw new ArgumentException("Server IP cannot be null or empty.");
            ServerIp = serverIp;
            try
            {
                _client = new TcpClient(ServerIp, SMTPPort);
                Debug.WriteLine("Connected to SMTP server.");
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
                    Debug.WriteLine("SMTP Service stopped.");
                }
                else
                {
                    Debug.WriteLine("SMTP Service is not running.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while stopping IMAP service: {ex.Message}");
            }
        }
    }
}

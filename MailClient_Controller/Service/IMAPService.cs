using System.Net.Sockets;

namespace MailClient_Controller.Service
{
    public class IMAPService
    {
        private static readonly Lazy<IMAPService> _instance = new(() => new IMAPService());
        public TcpClient _client;
        private const int ImapPort = 143;

        public string ServerIp { get; private set; } = string.Empty;

        private IMAPService() { }

        // Singleton Instance property.
        public static IMAPService Instance => _instance.Value;

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
                Console.WriteLine("Cannot start IMAP service. Server IP is not initialized.");
                return;
            }

            try
            {
                // Nếu _client là null, khởi tạo mới
                if (_client == null)
                {
                    _client = new TcpClient(ServerIp, ImapPort);
                    Console.WriteLine("IMAP Connected.");
                }
                else if (!_client.Connected)
                {
                    // Nếu _client đã được khởi tạo nhưng không còn kết nối, tạo lại kết nối
                    _client.Close();
                    _client = new TcpClient(ServerIp, ImapPort);
                    Console.WriteLine("IMAP Reconnected.");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Failed to connect to IMAP server: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while starting IMAP service: {ex}");
            }
        }

        public void StopService()
        {
            try
            {
                if (_client != null && _client.Connected)
                {
                    _client.Close();
                    Console.WriteLine("IMAP Service stopped.");
                }
                else
                {
                    Console.WriteLine("IMAP Service is not running.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while stopping IMAP service: {ex.Message}");
            }
        }
    }
}


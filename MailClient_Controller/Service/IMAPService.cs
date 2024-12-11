using System.Diagnostics;
using System.Net.Sockets;

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
            throw new ArgumentException("Server IP cannot be null or empty.");
        ServerIp = serverIp;
        try
        {
            _client = new TcpClient(ServerIp, ImapPort);
            Debug.WriteLine("Connected to IMAP server.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Initialize: {ex.Message}");
        }
    }
    // Dừng kết nối khi không cần thiết
    public void StopService()
    {
        try
        {
            if (_client != null && _client.Connected)
            {
                _client.Close();
                Debug.WriteLine("IMAP Service stopped.");
            }
            else
            {
                Debug.WriteLine("IMAP Service is not running.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while stopping IMAP service: {ex.Message}");
        }
    }
}



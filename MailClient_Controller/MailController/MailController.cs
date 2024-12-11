using MailClient_Controller.Enities;
using MailClient_Controller.Service;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace MailClient_Controller.MailController
{
    public class MailController
    {
        public string UserName { get; set; }
        private readonly IMAPService imapService = IMAPService.Instance;

        private List<Mail> SendRequest(object command)
        {
            List<Mail> mails = new List<Mail>();
            TcpClient client = imapService._client;
            if (client == null || !client.Connected)
            {
                client = imapService._client;
            }
            NetworkStream stream = null;
            StreamReader reader = null;
            StreamWriter writer = null;
            try
            {
                stream = IMAPService.Instance._client.GetStream();
                if (stream == null) return mails;

                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                string jsonCommand = JsonConvert.SerializeObject(command);
                writer.WriteLine(jsonCommand);

                var timeout = DateTime.Now.AddSeconds(10);
                while (DateTime.Now < timeout)
                {
                    if (stream.DataAvailable)
                    {
                        string response = reader.ReadLine();
                        if (response != null)
                        {
                            ServerResponse serverResponse = new ServerResponse();
                            serverResponse = ServerResponse.FromJson(response);
                            mails = serverResponse.Mails;
                            return mails;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendRequest: {ex.Message}");
            }
            return mails;
        }
        public MailController(string userName)
        {
            UserName = userName;
        }

        public List<Mail> fetchMail()
        {
            var selectCommand = new
            {
                Command = "SELECT",
                Mailbox = "INBOX"
            };
            return SendRequest(selectCommand);
        }
    }
}

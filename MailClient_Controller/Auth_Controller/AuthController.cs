using MailClient_Controller.Enities;
using MailClient_Controller.Service;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MailClient_Controller.Auth_Controller
{
    public class AuthController
    {
        private readonly IMAPService imapService = IMAPService.Instance;

        private bool SendRequest(object command)
        {
            imapService.StartService();
            TcpClient client = imapService._client;
            if (client == null || !client.Connected)
            {
                imapService.StartService();
                client = imapService._client;
            }
            NetworkStream stream = null;
            StreamReader reader = null;
            StreamWriter writer = null;

            try
            {
                stream = client.GetStream();
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
                            ServerResponse serverResponse = ServerResponse.FromJson(response);
                            if (serverResponse.Status.Contains("OK"))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        Task.Delay(100).Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendRequest: {ex.Message}");
            }
            finally
            {
                writer?.Dispose();
                reader?.Dispose();
                stream?.Dispose();
            }

            return false;
        }
        public bool Capability()
        {
            var capabilitycommand = new
            {
                Command = "CAPABILITY"
            };
            return SendRequest(capabilitycommand);
        }

        public bool SignUp(string username, string fullname, string password)
        {
            var registerCommand = new
            {
                Command = "REGISTER",
                Username = username,
                Fullname = fullname,
                Password = password
            };
            return SendRequest(registerCommand);
        }

        public bool SignIn(string username, string password)
        {
            var loginCommand = new
            {
                Command = "LOGIN",
                Username = username,
                Password = password
            };
            return SendRequest(loginCommand);
        }
    }
}

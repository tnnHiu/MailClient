using MailClient_Controller.Enities;
using MailClient_Controller.Service;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MailClient_Controller.Auth_Controller
{
    public class AuthController
    {
        private readonly IMAPService imapService = IMAPService.Instance;

       
        private bool SendRequest(string command, object parameters)
        {
            TcpClient client = imapService._client;
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            var request = new
            {
                Command = command,
                Data = parameters
            };

            string jsonCommand = JsonConvert.SerializeObject(request);
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

            return false;
        }

        public bool SignUp()
        {
            var registerCommand = new
            {
                Username = "vantn.21it",
                Fullname = "Tào Nguyên Văn",
                Password = "12345678"
            };
            return SendRequest("REGISTER", registerCommand);
        }

        public bool SignIn()
        {
            var loginCommand = new
            {
                Username = "vantn.21it",
                Password = "87654321"
            };
            return SendRequest("LOGIN", loginCommand);
        }
    }
}

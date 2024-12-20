using MailClient_Controller.Enities;
using MailClient_Controller.Service;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
namespace MailClient_Controller.Auth_Controller
{
    public class AuthController
    {
        IMAPService iMAPService = IMAPService.Instance;

        SMTPService sMTPService = SMTPService.Instance;
        FTPService fTPService = FTPService.Instance;


   
        private bool SendRequest(object command)
        {
            NetworkStream stream = null;
            StreamReader reader = null;
            StreamWriter writer = null;
            try
            {
                stream = iMAPService._client.GetStream();
                if (stream == null) return false;

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
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendRequest: {ex.Message}");
            }

            return false;
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

        //public bool SignOut()
        //{
        //    if ()
        //    {
        //        iMAPService.StopService();
        //        fTPService.StopService();
        //        sMTPService.StopService();          
        //        return true;
        //    }
        //  return false ;
        //}

    }
}


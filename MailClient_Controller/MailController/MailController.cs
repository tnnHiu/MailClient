using MailClient_Controller.Enities;
using MailClient_Controller.Service;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace MailClient_Controller.MailController
{
    public class MailController
    {
        public string UserName { get; set; }
        private readonly IMAPService imapService = IMAPService.Instance;
        private readonly SMTPService smtpService = SMTPService.Instance;


        private bool SendEmailRequest(object command)
        {
            NetworkStream stream = null;
            StreamReader reader = null;
            StreamWriter writer = null;
            try
            {
                stream = smtpService._client.GetStream();
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
                            Debug.WriteLine($"Server resplonse {serverResponse.Status}");
                            if (serverResponse.Status.Contains("OK"))
                            {
                                Debug.WriteLine($"true {jsonCommand}");
                                return true;
                            }
                            else
                            {
                                Debug.WriteLine($"false {jsonCommand}");
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

        public List<Mail> fetchSentMail()
        {
            var selectCommand = new
            {
                Command = "SELECT",
                Mailbox = "SENT"
            };
            return SendRequest(selectCommand);
        }


        public List<Mail> fetchtTrashMail()
        {
            var selectCommand = new
            {
                Command = "SELECT",
                Mailbox = "TRASH"
            };
            return SendRequest(selectCommand);
        }

        public List<Mail> fetchAllMail()
        {
            var selectCommand = new
            {
                Command = "SELECT",
                Mailbox = "ALL"
            };
            return SendRequest(selectCommand);
        }
        public void moveToTrash(int mailId)
        {
            var moveCommand = new
            {
                Command = "DELETE",
                Mailid = mailId
            };
            SendRequest(moveCommand);
        }


        public void restoreMail(int mailId)
        {
            var moveCommand = new
            {
                Command = "RESTORE",
                Mailid = mailId
            };
            SendRequest(moveCommand);
        }
        public bool SendEmail(Mail mail)
        {
            var mailFromCommand = new
            {
                Command = "MAIL FROM",
                Email = mail.Sender
            };

            var rcptToCommand = new
            {
                Command = "RCPT TO",
                Email = mail.Receiver
            };

            var dataCommand = new
            {
                Command = "DATA",
                Subject = mail.Subject,
                Content = mail.Content
            };
            return (SendEmailRequest(mailFromCommand) &&
                    SendEmailRequest(rcptToCommand) &&
                    SendEmailRequest(dataCommand));
        }

        public List<Mail> fetchMailDetail(int id)
        {
            var fetchMailDetailCommand = new
            {
                Command = "FETCH",
                Mailid = id
            };
            return SendRequest(fetchMailDetailCommand);
        }
    }
}

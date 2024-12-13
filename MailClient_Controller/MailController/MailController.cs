using MailClient_Controller.Enities;
using MailClient_Controller.Service;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;

namespace MailClient_Controller.MailController
{
    public class MailController
    {
        public string UserName { get; set; }
        public string FilePath { get; set; }

        public string ResponseIdentify { get; set; }
        private readonly IMAPService imapService = IMAPService.Instance;
        private readonly SMTPService smtpService = SMTPService.Instance;
        private readonly FTPService fTPService = FTPService.Instance;

        public MailController(string Username, string filePath)
        {
            UserName = Username;
            FilePath = filePath;
        }

        public MailController(string userName)
        {
            UserName = userName;
        }

        //public bool SendFile(Mail mail)
        //{
        //    NetworkStream stream = null;
        //    StreamReader reader = null;
        //    StreamWriter writer = null;
        //    try
        //    {
        //        stream = fTPService._client.GetStream();
        //        if (stream == null) return false;
        //        reader = new StreamReader(stream, Encoding.UTF8);
        //        writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

        //        var ftpCommand = new
        //        {
        //            Command = "FTP",
        //            Sender = mail.Sender,
        //            Recipient = mail.Receiver,
        //        };

        //        string jsonCommand = JsonConvert.SerializeObject(ftpCommand);
        //        writer.WriteLine(jsonCommand);
        //        string response = reader.ReadLine();


        //        var putCommand = new
        //        {
        //            Command = "PUT",
        //            Identify = ResponseIdentify,
        //            Filename = System.IO.Path.GetFileName(FilePath),
        //        };

        //        jsonCommand = JsonConvert.SerializeObject(ftpCommand);
        //        writer.WriteLine(jsonCommand);
        //        response = reader.ReadLine();


        //        using FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        //        byte[] buffer = new byte[64 * 1024];
        //        int bytesRead;

        //        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
        //        {
        //            stream.Write(buffer, 0, bytesRead);
        //        }
        //        stream.Flush();


        //        response = reader.ReadLine();
        //        if (response != null)
        //        {
        //            ServerResponse serverResponse = ServerResponse.FromJson(response);
        //            if (serverResponse.Status.Contains("OK"))
        //            {
        //                Debug.WriteLine($"true {jsonCommand}");
        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Error in SendFile: {ex.Message}");
        //    }
        //    return false;
        //}




        public bool SendFile(Mail mail)
        {
            try
            {
                using NetworkStream stream = fTPService._client.GetStream();
                if (stream == null) return false;

                //stream.ReadTimeout = 10000; 
                //stream.WriteTimeout = 10000; 

                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

      
                var ftpCommand = new
                {
                    Command = "FTP",
                    Sender = mail.Sender,
                    Recipient = mail.Receiver,
                };
                if (!SendCommandAndCheckResponse(writer, reader, ftpCommand)) return false;

         
                var putCommand = new
                {
                    Command = "PUT",
                    Identify = ResponseIdentify,
                    Filename = Path.GetFileName(FilePath),
                };
                if (!SendCommandAndCheckResponse(writer, reader, putCommand)) return false;

  
                SendFileData(stream);

                // Nhận phản hồi từ server
                string response = reader.ReadLine();
                if (response != null && ServerResponse.FromJson(response).Status.Contains("OK"))
                {
                    Debug.WriteLine("File sent successfully");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SendFile: {ex.Message}");
            }
            return false;
        }

        private bool SendCommandAndCheckResponse(StreamWriter writer, StreamReader reader, object command)
        {
            try
            {
                string jsonCommand = JsonConvert.SerializeObject(command);
                writer.WriteLine(jsonCommand);

                string response = reader.ReadLine();
                if (response == null) return false;

                ServerResponse serverResponse = ServerResponse.FromJson(response);
                return serverResponse.Status.Contains("OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SendCommandAndCheckResponse: {ex.Message}");
                return false;
            }
        }

        private void SendFileData(NetworkStream stream)
        {
            try
            {
                using FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[64 * 1024]; 
                int bytesRead;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, bytesRead);
                }
                stream.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SendFileData: {ex.Message}");
            }
        }




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
                            if (serverResponse.Status.Contains("OK") && (string.IsNullOrEmpty(serverResponse.Identify)))
                            {
                                Debug.WriteLine($"true {jsonCommand}");
                                return true;
                            }
                            else if (serverResponse.Status.Contains("OK") && (!string.IsNullOrEmpty(serverResponse.Identify)))
                            {
                                ResponseIdentify = serverResponse.Identify;
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
                Debug.WriteLine($"Error in SendRequest: {ex.Message}");
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


        public bool SendMailWithAttach(Mail mail)
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

            var attachCommand = new
            {
                Command = "ATTACH",
                Filename = mail.Attachment,
            };

            var dataCommand = new
            {
                Command = "DATA",
                Subject = mail.Subject,
                Content = mail.Content
            };
            return (SendEmailRequest(mailFromCommand) &&
                    SendEmailRequest(rcptToCommand) &&
                    SendEmailRequest(attachCommand) &&
                    SendEmailRequest(dataCommand));
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

        public bool ReplyEmail(Mail mail)
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

            var replyCommand = new
            {
                Command = "REPLY",
                Mailid = mail.Id,
            };

            var dataCommand = new
            {
                Command = "DATA",
                Subject = mail.Subject,
                Content = mail.Content
            };

            return (SendEmailRequest(mailFromCommand) &&
                   SendEmailRequest(rcptToCommand) &&
                   SendEmailRequest(replyCommand) &&
                   SendEmailRequest(dataCommand));
        }


        public bool ForwardEmail(Mail mail)
        {
            var mailFromCommand = new
            {
                Command = "FORWARD FROM",
                Email = mail.Sender
            };

            var rcptToCommand = new
            {
                Command = "FORWARD TO",
                Email = mail.Receiver
            };

            var mailForwardCommand = new
            {
                Command = "MAIL FORWARD",
                Mailid = mail.Id,
            };
            //var quitCommand = new
            //{
            //    Command = "QUIT"
            //};


            return (SendEmailRequest(mailFromCommand) &&
                   SendEmailRequest(rcptToCommand) &&
                   SendEmailRequest(mailForwardCommand));

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







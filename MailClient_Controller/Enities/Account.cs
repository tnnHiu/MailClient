using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClient_Controller.Enities
{
    public class Account
    {
        public string EmailAddress { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }

        // Default Constructor
        public Account()
        {
            EmailAddress = string.Empty;
            Fullname = string.Empty;
            Password = string.Empty;
            CreatedAt = DateTime.Now;
        }

        // Parameterized Constructor
        public Account(string emailAddress, string fullname, string password)
        {
            EmailAddress = emailAddress;
            Fullname = fullname;
            Password = password;
            CreatedAt = DateTime.Now;
        }
    }
}

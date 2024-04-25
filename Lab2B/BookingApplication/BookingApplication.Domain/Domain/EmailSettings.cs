using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Domain
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set;}
        public int SmtpServerPort { get; set; }
        public bool EnableSsl { get; set; }
        public string EmailDisplayName { get; set; }
        public string SendersName { get; set; }

        public EmailSettings()
        {
            
        }

        public EmailSettings(string smtpServer, string smtpUserName, string smtpPassword, int smptServerPort)
        {
            this.SmtpServer = smtpServer;
            this.SmtpUserName = smtpUserName;
            this.SmtpPassword = smtpPassword;
            this.SmtpServerPort = smptServerPort;
        }

    }
}

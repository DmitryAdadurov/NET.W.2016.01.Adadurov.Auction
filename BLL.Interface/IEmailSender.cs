using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IEmailSender
    {
        string Username { get; set; }
        string Password { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        bool SSLEnable { get; set; }

        string ToEmail { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        bool IsHtml { get; set; }

        Task Send();
    }
}

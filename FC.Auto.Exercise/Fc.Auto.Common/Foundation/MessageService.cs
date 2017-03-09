using Fc.Auto.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Fc.Auto.Common.Foundation
{
    public class MessageService : IMailService
    {
        private string _host = string.Empty;
        private int _port = 80;
        private string _senderAddress = string.Empty;
        private string _senderPassword = string.Empty;
        private bool _useDefault = true;
        public void SendMail(MailMessage mail)
        {
            using (var client = new SmtpClient(_host, _port))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = _useDefault;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(_senderAddress, _senderPassword);
                client.Send(mail);
            }
        }

        public void SetCredentials(string senderAddress, string senderPassword, bool useDefault)
        {
            _senderAddress = senderAddress;
            _senderPassword = senderPassword;
            _useDefault = useDefault;
        }

        public void SetHost(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public bool TrySendMail(MailMessage mail)
        {
            try
            {
                SendMail(mail);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}

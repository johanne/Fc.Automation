using System.Net.Mail;

namespace Fc.Auto.Common.Interface
{
    interface IMailService
    {
        void SetHost(string host, int port);
        void SetCredentials(string senderAddress, string senderPassword, bool useDefault);
        bool TrySendMail(MailMessage mail);
        void SendMail(MailMessage mail);
    }
}

using System.Net.Mail;

namespace Fc.Auto.Common.Interface
{
    /// <summary>
    /// Raw builder, no need for the complicated design
    /// since this is a one time implementation
    /// </summary>
    public interface IMailMessageBuilder
    {
        
        IMailMessageBuilder AddSender(string senderAddress, string displayName);
        IMailMessageBuilder AddToAddress(string receiverAddress, string displayName);
        IMailMessageBuilder AddSubject(string subject);
        IMailMessageBuilder AddBody(string body);
        MailMessage Build();

    }
}

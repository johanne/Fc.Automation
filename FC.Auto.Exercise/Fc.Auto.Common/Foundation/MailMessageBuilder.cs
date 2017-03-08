using Fc.Auto.Common.Interface;
using System;
using System.Net.Mail;

namespace Fc.Auto.Common.Foundation
{
    public class MailMessageBuilder : IMailMessageBuilder
    {

        private MailAddress _sender;
        private MailAddress _receiver;
        private string _subject;
        private string _body;
        private static IMailMessageBuilder _instance;
        public static IMailMessageBuilder Instance
        {
            get
            {
                return (_instance ?? (_instance = new MailMessageBuilder())) as IMailMessageBuilder;
            }
        }
        private MailMessageBuilder()
        {
        }

        public IMailMessageBuilder AddBody(string body)
        {
            _body = body;
            return this;
        }

        public IMailMessageBuilder AddSender(string senderAddress, string displayName)
        {
            _sender = new MailAddress(senderAddress, displayName);
            return this;
        }

        public IMailMessageBuilder AddSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public IMailMessageBuilder AddToAddress(string receiverAddress, string displayName)
        {
            _receiver = new MailAddress(receiverAddress, displayName);
            return this;
        }

        public MailMessage Build()
        {
            if (_sender == null)
            {
                throw new InvalidOperationException("Sender address not set.");
            }
            if (_receiver == null)
            {
                throw new InvalidOperationException("Receiver address not set.");
            }

            return new MailMessage(_sender, _receiver)
            {
                Subject = _subject,
                Body = _body,
            };
        }
    }
}

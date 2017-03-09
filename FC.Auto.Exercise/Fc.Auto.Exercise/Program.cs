using Fc.Auto.Common.Foundation;
using Fc.Auto.Common.Interface;
using System;
using System.Configuration;

namespace Fc.Auto.Exercise
{
    class Program
    {
        private static ILogger _logger;
        static void Main(string[] args)
        {
            InitializeComponents();


            bool result = false;
            var failCount = 0;
            using (var program = new FoodPanda())
            {
                result = program.Run();
                
            }
            if (result)
            {
                _logger.Log("Preparing to send email notification...");
                //mail here
                
                var emailSent = SendMail();

                if (emailSent)
                {
                    _logger.Log("Email Notification sent successfully!");
                }
                else
                {
                    _logger.LogE("There was a problem while sending the email!");
                    failCount++;
                }
                
            }
            else
            {
                _logger.LogE("There was an error in the automated script.");
                failCount++;
            }

            _logger.Log($@"Run completed. Failure statistics: {failCount}");

            System.Threading.Thread.Sleep(1000);
        }

        public static void InitializeComponents()
        {

            _logger = ClassLoader.Instance.CreateOrLocate<ILogger>(typeof(Logger));
            

        }

        public static bool SendMail()
        {
            var senderName = ConfigurationManager.AppSettings["senderName"];
            var senderAddress = ConfigurationManager.AppSettings["senderAddress"];
            var senderPassword = ConfigurationManager.AppSettings["senderPassword"];
            var receiverName = ConfigurationManager.AppSettings["receiverName"];
            var receiverAddress = ConfigurationManager.AppSettings["receiverAddress"];
            var host = ConfigurationManager.AppSettings["host"];
            var port = int.Parse(ConfigurationManager.AppSettings["port"]);
            var messageBuilder = MailMessageBuilder.Instance;
            var sender =
                messageBuilder.AddSender(senderAddress, senderName)
                    .AddToAddress(receiverAddress, receiverName)
                    .AddSubject($@"FoodPanda script - {DateTime.Now.ToShortTimeString()}")
                    .AddBody($@"Script was successfully run. Order is ready for checkout");
            using (var mail = messageBuilder.Build())
            {
                var mailer = new MessageService();
                mailer.SetHost(host, port);
                mailer.SetCredentials(senderAddress, senderPassword, false);
                return mailer.TrySendMail(mail);
            }
        }
    }
}

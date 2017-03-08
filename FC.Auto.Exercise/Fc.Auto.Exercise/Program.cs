using Fc.Auto.Common.Foundation;
using Fc.Auto.Common.Interface;


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
                var mailer = new object();
                var emailSent = false;

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

        }

        public static void InitializeComponents()
        {

            _logger = ClassLoader.Instance.CreateOrLocate<ILogger>(typeof(Logger));
            

        }
    }
}

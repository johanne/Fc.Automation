using Fc.Auto.Common.Interface;
using System;

namespace Fc.Auto.Common.Foundation
{
    public class Logger : ILogger
    {
        /// <summary>
        /// Basic log. For further implementation
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void LogE(string message)
        {
            Console.WriteLine("Error: {0}", message);
        }
    }
}

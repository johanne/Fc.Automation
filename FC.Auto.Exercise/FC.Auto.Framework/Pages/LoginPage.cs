using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace FC.Auto.Framework.Pages
{
    public class LoginPage : PageBase
    {

        [FindsBy(How = How.Id, Using = "customer_login_login")]
        private IWebElement _loginButton; 
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public LoginPage(IWebDriver driver, double waitTimeMillis) : base(driver, waitTimeMillis)
        {
        }

        protected override string _baseUrl
        {
            get
            {
                return _driver.Url;
            }
        }

        public override void WaitForElementToLoad()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_loginButton));
        }
    }
}

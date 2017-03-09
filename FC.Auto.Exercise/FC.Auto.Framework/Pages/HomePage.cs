using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace FC.Auto.Framework.Pages
{
    public class HomePage : PageBase
    {
        [FindsBy(How = How.Id, Using = "button")]
        private IWebElement _submitButton;
        [FindsBy(How = How.Id, Using = "address")]
        private IWebElement _addressBar;
        [FindsBy(How = How.Id, Using = "wrapper-element-1")]
        private IWebElement _ddl;
        
        private By _spinner = By.CssSelector("img[id='find-restaurant-spinner']");
        private By _cityInput = By.CssSelector("span > input.form-control.twitter-typeahead.tt-input");
        private By _addressDdl = By.CssSelector("div[class='tt-suggestion']");
        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        public HomePage(IWebDriver driver, double waitTimeMillis) : base(driver, waitTimeMillis)
        {
        }

        protected override string _baseUrl
        {
            get
            {
                return "https://www.foodpanda.ph/";
            }
        }

        public override void WaitForElementToLoad()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_submitButton));
        }

        /// <summary>
        /// Sets the Drop Down Selector to the specified parameter
        /// </summary>
        /// <param name="city"></param>
        public void SetCity(string city)
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_ddl)).Click();

            var input = _ddl.FindElement(_cityInput);
            input.Clear();
            input.SendKeys(city);

            var select = new SelectElement(_driver.FindElement(By.Id("cityId")));
            select.SelectByIndex(1);

            input.SendKeys(Keys.Tab);
        }

        public void ClickSearch()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_submitButton)).Click();
        }

        public void WaitForSpinnerToDisappear()
        {
            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(_spinner));
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_spinner));
        }

        public void SetStreet(string streetName)
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_addressBar));

            // wait here to know that there is a match!
            
            _addressBar.SendKeys(streetName);

            _wait.Until(ExpectedConditions.ElementIsVisible(_addressDdl));
            _addressBar.SendKeys(Keys.Tab);
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_addressDdl));
        }
    }
}

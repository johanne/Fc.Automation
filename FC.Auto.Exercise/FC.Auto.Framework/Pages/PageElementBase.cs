using FC.Auto.Framework.Constants;
using FC.Auto.Framework.Interface;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;

namespace FC.Auto.Framework.Pages
{
    public abstract class PageElementBase : IPageElement
    {
        protected readonly IWebDriver _driver;
        protected readonly WebDriverWait _wait;

        /// <summary>
        /// Hide the default constructor
        /// </summary>
        private PageElementBase()
        {
        }

        /// <summary>
        /// Initializes a page with the default wait time <see cref="Constants.PageConstants.PageWaitDefaultMillis"/>
        /// </summary>
        /// <param name="driver"></param>
        public PageElementBase(IWebDriver driver) : this(driver, PageConstants.PageWaitDefaultMillis)
        {

        }

        public PageElementBase(IWebDriver driver, double waitTimeMillis)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(waitTimeMillis));
            PageFactory.InitElements(_driver, this);
        }

        public abstract void WaitForElementToLoad();
    }
}

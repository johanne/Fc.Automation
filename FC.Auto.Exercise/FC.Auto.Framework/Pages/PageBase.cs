using FC.Auto.Framework.Constants;
using FC.Auto.Framework.Interface;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Auto.Framework.Pages
{
    public abstract class PageBase : IPage
    {
        protected readonly IWebDriver _driver;
        protected readonly WebDriverWait _wait;
        protected abstract string _baseUrl { get; }
        /// <summary>
        /// Hide the default constructor
        /// </summary>
        private PageBase()
        {
        }

        /// <summary>
        /// Initializes a page with the default wait time <see cref="Constants.PageConstants.PageWaitDefaultMillis"/>
        /// </summary>
        /// <param name="driver"></param>
        public PageBase(IWebDriver driver) : this(driver, PageConstants.PageWaitDefaultMillis)
        {

        }

        public PageBase(IWebDriver driver, double waitTimeMillis)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(waitTimeMillis));
            PageFactory.InitElements(_driver, this);
        }

        /// <summary>
        /// Refresh the current page.
        /// </summary>
        public void Refresh()
        {
            _driver.Navigate().GoToUrl(_driver.Url);
        }

        /// <summary>
        /// Waits for specific elements to ensure that the
        /// page has already loaded.
        /// </summary>
        public abstract void WaitForPageToLoad();

        /// <summary>
        /// Navigates to the page's URL, if defined
        /// </summary>
        public virtual void Navigate()
        {
            _driver.Navigate().GoToUrl(_baseUrl);
        }
    }
}

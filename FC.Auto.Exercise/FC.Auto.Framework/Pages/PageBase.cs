using FC.Auto.Framework.Interface;
using OpenQA.Selenium;

namespace FC.Auto.Framework.Pages
{
    public abstract class PageBase : PageElementBase, IPage
    {

        protected abstract string _baseUrl { get; }


        public PageBase(IWebDriver driver) : base(driver)
        {
        }

        public PageBase(IWebDriver driver, double waitTimeMillis) : base(driver, waitTimeMillis)
        {
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
        public abstract override void WaitForElementToLoad();

        /// <summary>
        /// Navigates to the page's URL, if defined
        /// </summary>
        public virtual void Navigate()
        {
            _driver.Navigate().GoToUrl(_baseUrl);
        }
    }
}

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
    public class OrderPage : PageBase
    {
        private const string _itemContainerCss = "div[class='vendor-body-container']";
        [FindsBy(How = How.CssSelector, Using = _itemContainerCss)]
        private IWebElement _itemContainer;

        public OrderPage(IWebDriver driver) : base(driver)
        {
        }

        public OrderPage(IWebDriver driver, double waitTimeMillis) : base(driver, waitTimeMillis)
        {
        }

        protected override string _baseUrl
        {
            get
            {
                return _driver.Url;
            }
        }

        public override void WaitForPageToLoad()
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(_itemContainerCss)));
        }

        public class MenuModal
        {

        }

        private class MenuItem
        {

        }
    }
}

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
    public class RestaurantsPage : PageBase
    {
        [FindsBy(How = How.CssSelector, Using = "section[class='js-infscroll-load-more-here']")]
        private IWebElement _restaurantList;
        [FindsBy(How = How.CssSelector, Using = "div[class='characteristics'] > ul[class='filter__list']")]
        private IWebElement _filterList;
        [FindsBy(How = How.CssSelector, Using = "div[class='cuisines'] > ul[class='filter__list']")]
        private IWebElement _cuisineList;

        

        public RestaurantsPage(IWebDriver driver) : base(driver)
        {
        }

        public RestaurantsPage(IWebDriver driver, double waitTimeMillis) : base(driver, waitTimeMillis)
        {
        }

        public void SelectFilterByName(string filterName)
        {
            var fact = new ListItemFactory(_driver, _filterList);
            var listItems = fact.GetListItems();


        }

        protected override string _baseUrl
        {
            get
            {
                return "https://www.foodpanda.ph/restaurants";
            }
        }

        public override void WaitForPageToLoad()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_restaurantList));
        }

        private class ListItemFactory
        {
            private IWebElement _anchor;
            private IWebDriver _driver;
            private By _listItems = By.CssSelector("li[class=;filter__list__item']");

            /// <summary>
            /// Hide default constructor
            /// </summary>
            private ListItemFactory()
            {
            }
            public ListItemFactory(IWebDriver driver, IWebElement anchor)
            {
                _driver = driver;
                _anchor = anchor;
            }

            public List<ListItem> GetListItems()
            {
                var elements = _anchor.FindElements(_listItems);
                return elements.Select(x => new ListItem(_driver)).ToList();
            }
        }

        private class ListItem
        {
            private IWebDriver _driver;
            private WebDriverWait _wait;
            [FindsBy(How = How.CssSelector, Using = "input[type = 'checkbox']")]
            public IWebElement CheckBox;
            [FindsBy(How = How.CssSelector, Using = "label[type='required']")]
            public IWebElement Label;

            public readonly By CheckBoxBy = By.CssSelector("input[type = 'checkbox']");
            public readonly By LabelBy = By.CssSelector("label[type='required']");
            public ListItem(IWebDriver driver) : this(driver, 5000)
            {
            }

            public ListItem(IWebDriver driver, double waitTimeMillis)
            {
                _driver = driver;
                _wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(waitTimeMillis));
                PageFactory.InitElements(_driver, this);
            }

            public void SetSelected()
            {
                if (!CheckBox.Selected)
                {
                    CheckBox.Click();
                }
            }
        }
    }

    

}

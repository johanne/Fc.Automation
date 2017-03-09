using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

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
        [FindsBy(How = How.CssSelector, Using = "select[name='sort']")]
        private IWebElement _filterSort;

        private By _restaurantItem = By.CssSelector("article[class='vendor list js-vendor-list-vendor-panel']");

        private readonly Dictionary<SortOptions, string> _sortOptionsDictionary = new Dictionary<SortOptions, string>()
        {
            {SortOptions.DeliveryFee, "Deliery fee" },
            {SortOptions.FastestDelivery, "Fastest delivery" },
            {SortOptions.MinimumOrderValue, "Minimum order value" },
            {SortOptions.Ratings, "Ratings" },
            {SortOptions.Relevance, "Relevance" },

        };

        public RestaurantsPage(IWebDriver driver) : base(driver)
        {
        }

        public RestaurantsPage(IWebDriver driver, double waitTimeMillis) : base(driver, waitTimeMillis)
        {

        }

        public void SelectResult(int index)
        {
            var items = _restaurantList.FindElements(_restaurantItem);

            if (items.Count == 0)
            {
                throw new IndexOutOfRangeException("No restaurants were found for the selected criteria");
            }

            index = Math.Max(0, index);
            index = Math.Min(index, items.Count - 1);

            items.ElementAt(index).Click();

        }

        public void SelectTopResult()
        {
            var items = _restaurantList.FindElements(_restaurantItem);

            if (items.Count == 0)
            {
                throw new IndexOutOfRangeException("No restaurants were found for the selected criteria");
            }

            items.First().Click();
        }

        public void SelectFiltersByName(params string[] filterNames)
        {
            var elems = _filterList.FindElements(By.CssSelector("li[class='filter__list__item']"));
            var fact = new ListItemFactory(_driver, _filterList);
            var listItems = fact.GetListItems();

            foreach (var filter in filterNames)
            {
                var selected = listItems.FirstOrDefault(x => x.Label.Contains(filter));
                selected.SetSelected();
                WaitForElementToLoad();
            }
        }

        public void SortResults(SortOptions option)
        {
            _filterSort.Click();
            var selectOptions = new SelectElement(_filterSort);
            selectOptions.SelectByText(_sortOptionsDictionary[option]);
            WaitForElementToLoad();
        }

        protected override string _baseUrl
        {
            get
            {
                return "https://www.foodpanda.ph/restaurants";
            }
        }

        public override void WaitForElementToLoad()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_restaurantList));
        }

        public enum SortOptions
        {
            Relevance,
            Ratings,
            MinimumOrderValue,
            DeliveryFee,
            FastestDelivery,
        }
        private class ListItemFactory
        {
            private IWebElement _anchor;
            private IWebDriver _driver;
            private By _listItems = By.CssSelector("li[class='filter__list__item']");
            public readonly By CheckBoxBy = By.CssSelector("input[type='checkbox']");
            public readonly By LabelBy = By.TagName("label");

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
                return elements.Select(x => 
                    new ListItem(_driver){
                        CheckBox = x.FindElement(CheckBoxBy),
                        LabelElement = x.FindElement(LabelBy),
                }).ToList();
            }
        }

        private class ListItem
        {
            private IWebDriver _driver;
            private WebDriverWait _wait;
            public IWebElement CheckBox;
            public string Label => LabelElement.Text;
            public IWebElement LabelElement;
            public ListItem(IWebDriver driver) : this(driver, 5000)
            {
            }

            public ListItem(IWebDriver driver, double waitTimeMillis)
            {
                _driver = driver;
                _wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(waitTimeMillis));
            }

            public void SetSelected()
            {
                if (!CheckBox.Selected)
                {
                    LabelElement.Click();
                }
            }
        }
    }



}

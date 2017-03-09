using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using FC.Auto.Framework.Interface;

namespace FC.Auto.Framework.Pages
{
    public class OrderPage : PageBase
    {
        private const string _itemContainerCss = "div[class='vendor-body-container']";
        [FindsBy(How = How.CssSelector, Using = _itemContainerCss)]
        private IWebElement _itemContainer;

        [FindsBy(How = How.CssSelector, Using = "section[class='menu']")]
        private IWebElement _menuContainer;

        [FindsBy(How = How.CssSelector, Using = "div[class='cart__summary']")]
        private IWebElement _summaryContainer;

        [FindsBy(How = How.CssSelector, Using = "div[class='cart__checkout'] > a")]
        private IWebElement _checkoutButton;

        private By _spinner = By.CssSelector("div[class='spinner']");
        private By _subTotal = By.CssSelector("tr[class='subtotal']");
        private By _deliveryFee = By.CssSelector("tr[class='delivery-fee ']");
        private By _total = By.CssSelector("tr[class='total'] > *div");
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

        public override void WaitForElementToLoad()
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(_itemContainerCss)));

        }

        public void AddOrder(string orderName, string price, int numItems = 1)
        {
            // either check if there's a modal,
            // otherwise, keep mashing the plus button
            var fact = new MenuItemFactory(_driver, _menuContainer);
            var listItems = fact.GetListItems();

            var selectedItem = listItems.FirstOrDefault(x => x.MenuLabel.Equals(orderName));

            if (selectedItem != null)
            {
                // do the loop here
                for (int i = 0; i < numItems; i++)
                {
                    AddEndToEnd(selectedItem, price);
                }
            }
        }

        private void AddEndToEnd(MenuItem item, string price)
        {
            item.ClickAddButton(price);

            var menuModal = new MenuModal(_driver);

            try { menuModal.WaitForElementToLoad();
                menuModal.ClickContinueButton();
                
            }
            catch
            { // no modal exists
            }
            WaitForSpinnerToDisappear();
            WaitForSpinnerToDisappear();
            WaitForElementToLoad();
        }

        public string GetSubtotal()
        {
            try
            {
                return _summaryContainer.FindElement(_subTotal).Text;
            }
            catch
            {
            }
            return string.Empty;
        }
        public string GetDeliveryFee()
        {
            try
            {
                return _summaryContainer.FindElement(_deliveryFee).Text;
            }
            catch
            {
            }
            return string.Empty;

        }

        public string GetTotal()
        {
            try
            {
                return _summaryContainer.FindElement(_total).Text;
            }
            catch
            {
            }
            return string.Empty;

        }

        public void GetOrderReceipt()
        {
            // stub, this should return the list of orders
            throw new NotImplementedException();
        }

        public void WaitForSpinnerToDisappear()
        {
            try
            {
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(_spinner));
                _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_spinner));
            }
            catch { }
        }
        public List<MenuItem> GetMenuItemNames(int count)
        {
            var fact = new MenuItemFactory(_driver, _menuContainer);
            // return the menu items here to
            // determine the price and the label
            return fact.GetListItems(count);
        }

        public void Checkout()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_checkoutButton)).Click();
        }

        public bool IsCheckoutAllowed()
        {
            bool retVal = false;
            try
            {

                retVal = bool.Parse(_checkoutButton.GetAttribute("data-is-minimum-order-value-reached"));
            }
            catch
            {
                // do nothing, retVal is instantiated to false
            }
            return retVal;
        }

        public class MenuModal : PageElementBase
        {
            [FindsBy(How = How.Id, Using = "cart_product_skeleton_quantity")]
            private IWebElement _quantityField;

            [FindsBy(How = How.Id, Using = "cart_product_skeleton_submit")]
            private IWebElement _continueButton;

            public MenuModal(IWebDriver driver) : base(driver)
            {
            }

            public MenuModal(IWebDriver driver, double waitTimeMillis) : base(driver, waitTimeMillis)
            {
            }

            public override void WaitForElementToLoad()
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(_continueButton));
            }

            public void SetQuantity(int num)
            {
                _quantityField.Clear();
                _quantityField.SendKeys("num");
            }

            public void ClickContinueButton()
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(_continueButton))
                    .Click();
            }
        }

        public class MenuItem : PageElementBase
        {
            private Dictionary<string, IWebElement> _menuVariations = new Dictionary<string, IWebElement>();

            public string MenuLabel;

            public MenuItem(IWebDriver driver) : base(driver)
            {
            }

            public MenuItem(IWebDriver driver, double waitTimeMillis) : base(driver, waitTimeMillis)
            {
            }

            public void AddMenuVariation(string price, IWebElement button)
            {
                // brute approach
                if (!_menuVariations.ContainsKey(price))
                {
                    _menuVariations.Add(price, button);
                }
            }

            public override void WaitForElementToLoad()
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(_menuVariations.Values.First()));
            }

            public List<string> GetMenuVariations()
            {
                return _menuVariations.Keys.ToList();
            }

            public void ClickAddButton(string menuPrice)
            {
                if (_menuVariations.ContainsKey(menuPrice))
                {
                    var button = _menuVariations[menuPrice];
                    _wait.Until(ExpectedConditions.ElementToBeClickable(button)).Click();
                }
                else
                {
                    // throw?
                }
            }
        }

        private class MenuItemFactory
        {
            private IWebElement _anchor;
            private IWebDriver _driver;
            private readonly By _labelBy = By.CssSelector("div[class='menu-item__title']");
            private readonly By _menuItemContainer = By.CssSelector("article > article");
            private readonly By _variationContainer = By.CssSelector("ul[class='menu-item__variations'] > article");
            private readonly By _variationPrice = By.CssSelector("div[class='menu-item__variation__price ']");
            private readonly By _addButton = By.CssSelector("button[class='btn add-to-cart '] > i");
            /// <summary>
            /// Hide default constructor
            /// </summary>
            private MenuItemFactory()
            {
            }
            public MenuItemFactory(IWebDriver driver, IWebElement anchor)
            {
                _driver = driver;
                _anchor = anchor;
            }

            public List<MenuItem> GetListItems(int count = 10)
            {
                var retVal = new List<MenuItem>();
                var elements = _anchor.FindElements(_menuItemContainer).Take(count);

                foreach (var element in elements)
                {
                    var menuItem = new MenuItem(_driver)
                    {
                        MenuLabel = element.FindElement(_labelBy).Text
                    };
                    var variations = element.FindElements(_variationContainer);
                    foreach (var variation in variations)
                    {
                        menuItem.AddMenuVariation(variation.FindElement(_variationPrice).Text, variation.FindElement(_addButton));
                    }
                    // add menuVariation
                    retVal.Add(menuItem);
                }

                return retVal;
            }
        }
    }
}

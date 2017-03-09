using CsvHelper;
using Fc.Auto.Common.Foundation;
using Fc.Auto.Common.Interface;
using FC.Auto.Framework.Constants;
using FC.Auto.Framework.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using static FC.Auto.Framework.Pages.RestaurantsPage;

namespace Fc.Auto.Exercise
{
    /// <summary>
    /// This will be the complete program, I don't like running
    /// the whole thing via Program.cs
    /// </summary>
    class FoodPanda : IDisposable
    {
        const string OutputFileName = "order.csv";
        private ILogger _logger;
        private IWebDriver _webDriver;
        private HomePage _homePage;
        private RestaurantsPage _restaurants;
        private OrderPage _orders;
        private LoginPage _login;
        private double _waitTime;
        private string _city;
        private string _street;
        private string _filter;
        private string _sortOption;
        private string _output;

        public FoodPanda()
        {
            LoadAppSettings();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _homePage = ClassLoader.Instance.CreateOrLocate<HomePage>(typeof(HomePage), _webDriver, _waitTime);
            _restaurants = ClassLoader.Instance.CreateOrLocate<RestaurantsPage>(typeof(RestaurantsPage), _webDriver, _waitTime);
            _orders = ClassLoader.Instance.CreateOrLocate<OrderPage>(typeof(OrderPage), _webDriver, _waitTime);
            _login = ClassLoader.Instance.CreateOrLocate<LoginPage>(typeof(LoginPage), _webDriver, _waitTime);
        }

        public bool Run()
        {
            try
            {
               
                NavigateToHomePage();
                SetLocation();
                Search();
                SetFilterAndSelectARestaurant();
                AddMenuItems();
                // the next step should wait for the login page
                Checkout();
            }
            catch(Exception ex)
            {
                _logger.LogE($@"Error in running the steps: {ex.Message}");
                return false;
            }
            return true;
        }

        private void NavigateToHomePage()
        {
            _logger.Log("Navigating to home page..");
            _homePage.Navigate();
            _homePage.WaitForElementToLoad();
        }

        private void SetLocation()
        {
            _logger.Log($@"Searching for City: {_city}");
            _homePage.SetCity(_city);
            _logger.Log($@"Searching for City: {_street}");
            _homePage.SetStreet(_street);
            // check if it can be 
        }

        private void Search()
        {
            _homePage.ClickSearch();
            // ensure that a spinner is shown
            _homePage.WaitForSpinnerToDisappear();

            // wait for the restaurant results page to be shown
            _restaurants.WaitForElementToLoad();
            _logger.Log($@"Restaurant Page Loaded.");
        }

        private void SetFilterAndSelectARestaurant()
        {
            _logger.Log($@"Setting filter: {_filter}");
            _restaurants.SelectFiltersByName(_filter);

            SortOptions sortOption;
            if (!string.IsNullOrEmpty(_sortOption) && Enum.TryParse(_sortOption, out sortOption))
            {
                _logger.Log($@"Sorting via: {_sortOption}");
                _restaurants.SortResults(sortOption);
            }

            _restaurants.SelectResult(2);
            _orders.WaitForElementToLoad();
            _logger.Log($@"Order Page Loaded.");

        }

        private void AddMenuItems()
        {
            var items = _orders.GetMenuItemNames(1);
            var selectedItem = items.First();
            var price = selectedItem.GetMenuVariations().First();
            int count = 0;
            do
            {
                // This is a brute force method of just achieving 
                // the mvp required for food panda
                // add order count, order name, order price
                // add delivery charge
                // add total

                _logger.Log($"Adding order: {selectedItem.MenuLabel}, {price}");
                _orders.AddOrder(selectedItem.MenuLabel, price);
                count++;
            } while (!_orders.IsCheckoutAllowed());


            // will need to write the item to a csv file
            // item count, label, price
            // print the whole thing
            _logger.Log($"Logging order...");

            using (var sr = new StreamWriter(new FileStream(_output, FileMode.Create), Encoding.UTF8))
            {
                var writer = new CsvWriter(sr);

                // brute headers, no time to create type
                writer.WriteField("Quantity");
                writer.WriteField("Label");
                writer.WriteField("Price/Item");

                writer.NextRecord();
                writer.WriteField(count.ToString());

                writer.WriteField(selectedItem.MenuLabel);
                writer.WriteField(price);

                writer.NextRecord();
                writer.WriteField(_orders.GetSubtotal());
                writer.NextRecord();
                writer.WriteField(_orders.GetDeliveryFee());
                writer.NextRecord();
                writer.WriteField(_orders.GetTotal());
                writer.NextRecord();
                _logger.Log($"Log created at {_output}");
            }

        }

        private void Checkout()
        {
            

            
            _logger.Log($"Checking out order...");
            _orders.Checkout();

            _login.WaitForElementToLoad();
            _logger.Log($"Successful navigation to login page!");
        }

        private void LoadAppSettings()
        {
            // Load variable values
            _city = ConfigurationManager.AppSettings["city"];
            _street = ConfigurationManager.AppSettings["street"];
            _filter = ConfigurationManager.AppSettings["filter"];
            _sortOption = ConfigurationManager.AppSettings["sortOption"];

            _output = ConfigurationManager.AppSettings["outputLocation"];

            if (string.IsNullOrEmpty(_output))
            {
                _output = Path.GetTempPath();
            }

            _output = Path.Combine(Path.GetFullPath(_output), OutputFileName);
            
            // Load the webdriver type
            var type = ConfigurationManager.AppSettings["driver"];
            DriverType resultingType;

            if (!Enum.TryParse(type, out resultingType))
            {
                throw new ConfigurationErrorsException($"There was a problem reading the config section labeled \"driver\". Value: {resultingType}");
            }

            _webDriver =  ClassLoader.Instance.CreateOrLocate<IWebDriver>(typeof(ChromeDriver));

            if (!double.TryParse(ConfigurationManager.AppSettings["waitTime"], out _waitTime))
            {
                _waitTime = PageConstants.PageWaitDefaultMillis;
            }

            _logger = ClassLoader.Instance.CreateOrLocate<ILogger>(typeof(Logger));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _webDriver.Dispose();
                    _webDriver = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FoodPanda() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

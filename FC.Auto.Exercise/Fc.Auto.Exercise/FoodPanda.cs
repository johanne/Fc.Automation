using Fc.Auto.Common.Foundation;
using Fc.Auto.Common.Interface;
using FC.Auto.Framework.Constants;
using FC.Auto.Framework.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using static FC.Auto.Framework.Pages.RestaurantsPage;

namespace Fc.Auto.Exercise
{
    /// <summary>
    /// This will be the complete program, I don't like running
    /// the whole thing via Program.cs
    /// </summary>
    class FoodPanda : IDisposable
    {
        private ILogger _logger;
        private IWebDriver _webDriver;
        private HomePage _homePage;
        private RestaurantsPage _restaurants;
        private OrderPage _orders;
        private double _waitTime;
        private string _city;
        private string _street;
        private string _filter;
        private string _sortOption;

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
            _homePage.WaitForPageToLoad();
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
            _restaurants.WaitForPageToLoad();
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

            _restaurants.SelectTopResult();
            _orders.WaitForPageToLoad();
            _logger.Log($@"Order Page Loaded.");

        }

        private void AddMenuItems()
        {

        }

        private void LoadAppSettings()
        {
            // Load variable values
            _city = ConfigurationManager.AppSettings["city"];
            _street = ConfigurationManager.AppSettings["street"];
            _filter = ConfigurationManager.AppSettings["filter"];
            _sortOption = ConfigurationManager.AppSettings["sortOption"];
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

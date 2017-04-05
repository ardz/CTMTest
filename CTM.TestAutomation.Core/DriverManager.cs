namespace CTM.TestAutomation.Core
{
    using System.Collections.Generic;
    using System;

    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;

    public class DriverManager
    {
        /// <summary>
        /// the driver interface
        /// </summary>
        private static IWebDriver _driver;

        /// <summary>
        /// java script executor interface
        /// </summary>
        private static IJavaScriptExecutor _javaScriptExecutor;

        /// <summary>
        /// The drivers implicit wait value
        /// </summary>
        private static int _impWaitTimeout;

        /// <summary>
        /// The name of the browser the driver manager is using
        /// </summary>
        public static BrowserTypes BrowserName;

        /// <summary>
        /// The chrome driver service
        /// </summary>
        private static ChromeDriverService _service;

        /// <summary>
        /// The driver interface
        /// </summary>
        /// <returns></returns>
        public IWebDriver Driver
        {
            get
            {
                if (_driver != null)
                {
                    // driver exists, return it
                    return _driver;
                }

                _driver = Create(BrowserName); // it's null so create a new one

                // apply the imp wait timeout value
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_impWaitTimeout);

                return _driver;
            }

            set { _driver = value; }
        }

        /// <summary>
        /// Constructor for driver manager
        /// </summary>
        /// <param name="name"></param>
        /// <param name="impWaitTimeout"></param>
        public DriverManager(BrowserTypes name, int impWaitTimeout)
        {
            BrowserName = name;
            _impWaitTimeout = impWaitTimeout;
        }

        public static IWebElement FindWebElement(By locator)
        {
            return _driver.FindElement(locator);
        }

        public static IEnumerable<IWebElement> FindWebElements(By locator)
        {
            return _driver.FindElements(locator);
        }

        /// <summary>
        /// Waits for an element to appear, catch no such element exception if it
        /// doesn't appear within the max wait time
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="maxWaitInSeconds"></param>
        public static bool WaitForElement(By locator, int maxWaitInSeconds)
        {
            // BUG with webdriver wait always times out after 60 seconds?

            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(maxWaitInSeconds));

                try
                {
                    return wait.Until(drv => drv.FindElement(locator).Displayed);
                }
                catch (NoSuchElementException noSuchElement)
                {
                    //log error ("Element did not appear. " + noSuchElement);

                    return false;
                }

                // return true;
            }
            catch (WebDriverTimeoutException timeoutException)
            {
                // log error ("Timed out before element appeared " + timeoutException);

                return false;
            }
        }

        public static ChromeDriverService CreateChromeDriverService()
        {
            if (_service != null)
            {
                return _service;
            }

            _service = ChromeDriverService.CreateDefaultService(TestManager.GetBuildPath());

            return _service;
        }

        /// <summary>
        /// Creates a new driver interface or returns an already active one
        /// </summary>
        /// <param name="browserName"></param>
        /// <returns></returns>
        private static IWebDriver Create(BrowserTypes browserName)
        {
            _driver = null;

            switch (browserName)
            {
                case BrowserTypes.Firefox:
                {
                    _driver = new FirefoxDriver();
                }

                    break;

                case BrowserTypes.Chrome:
                {
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments(
                        "test-type",
                        "chrome.switches",
                        "--disable-extensions",
                        "--start-maximized");

                    // dismiss the 'do you want to save username and password' dialogue
                    chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                    chromeOptions.AddUserProfilePreference("password_manager_enabled", false);

                    try
                    {
                        _driver = new ChromeDriver(CreateChromeDriverService(), chromeOptions,
                            TimeSpan.FromSeconds(_impWaitTimeout));

                    }
                    catch (UnhandledAlertException)
                    {
                        _driver?.SwitchTo().Alert().Accept();
                    }
                }

                    break;

                case BrowserTypes.Edge:
                {
                    // todo check if edge has a selenium driver nuget package
                    break;
                }

                case BrowserTypes.Ie:

                    var ieOptions = new InternetExplorerOptions
                    {
                        UnexpectedAlertBehavior =
                            InternetExplorerUnexpectedAlertBehavior.Accept,
                        EnableNativeEvents = true,
                        RequireWindowFocus = true,
                        IntroduceInstabilityByIgnoringProtectedModeSettings
                            = true
                    };

                    _driver = new InternetExplorerDriver(ieOptions);

                    break;

                default:
                {
                    // log infomation ("No driver type specified defaulting to firefox");
                    _driver = new FirefoxDriver();
                }

                    break;
            }

            _driver?.Manage().Window.Maximize();

            return _driver;
        }

        /// <summary>
        /// Closes the driver interface
        /// </summary>
        public void Quit()
        {
            _driver.Quit();
        }
    }
}
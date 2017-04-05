namespace CTM.TestAutomation.Core
{
    using System;
    using System.Threading;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public abstract class BasePage
    {
        /// <summary>
        /// Base Page Url
        /// </summary>
        public string BasePageUrl { get; set; }

        /// <summary>
        /// Base Url of Application
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Driver Manager
        /// </summary>
        public DriverManager DriverManager { get; set; }

        /// <summary>
        /// Returns the url of a page
        /// </summary>
        /// <returns></returns>
        public virtual string PageUrl => this.BaseUrl + this.BasePageUrl;

        /// <summary>
        /// Constructor for base page
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="basePageUrl"></param>
        /// <param name="driver"></param>
        protected BasePage(string baseUrl, string basePageUrl, DriverManager driver)
        {
            this.BaseUrl = baseUrl;
            this.BasePageUrl = basePageUrl;
            this.DriverManager = driver;
        }

        /// <summary>
        /// Navigates to the page
        /// </summary>
        /// <param name="automaticallyAcceptAlert"></param>
        /// <returns></returns>
        public virtual bool NavigateTo(bool automaticallyAcceptAlert = true)
        {
            var complete = false;

            try
            {
                this.DriverManager.Driver.Navigate().GoToUrl(this.PageUrl);
                complete = this.IsPageLoaded();
            }
            catch (NoSuchElementException noSuchElementException)
            {
                //log error(
                //    $"Unexpected NoSuchElementException when attempting to navigate to:  {this.PageUrl}",
                //    nse);
            }
            catch (UnhandledAlertException unhandledAlertException)
            {
                // UnhandledAlertException handles the Chrome-related "Unexpected alert open" message.
                // http://stackoverflow.com/questions/19173195/how-to-handle-the-unexpected-alert-open
                if (automaticallyAcceptAlert)
                {
                    var alertText = this.DriverManager.Driver.SwitchTo().Alert().Text;
                    this.DriverManager.Driver.SwitchTo().Alert().Accept();
                    //log info($"NavigateTo() encounted and accepted Alert with Text: {alertText}");
                    //log info(unhandledAlertException.Message);
                }
                else throw;
            }

            return complete;
        }

        /// <summary>
        /// Returns true if the page is loaded successfully
        /// </summary>
        /// <returns></returns>
        public virtual bool IsPageLoaded()
        {
            var loaded = false;

            Thread.Sleep(2000);

            var handles = this.DriverManager.Driver.WindowHandles;

            try
            {
                foreach (var handle in handles)
                {
                    // Handle problem in IE driver when Switching to the current handle
                    if (DriverManager.BrowserName == BrowserTypes.Ie)
                    {
                        if (DriverManager.Driver.CurrentWindowHandle == handle)
                        {
                            this.DriverManager.Driver.SwitchTo().DefaultContent();

                            if (DriverManager.Driver.Url.ToLower().Contains(this.PageUrl.ToLower()))
                            {
                                loaded = true;
                            }
                        }
                        else if (
                            DriverManager.Driver.SwitchTo()
                                .Window(handle)
                                .Url.ToLower()
                                .Contains(this.PageUrl.ToLower()))
                        {
                            loaded = true;
                        }
                    }
                    else if (
                        DriverManager.Driver.SwitchTo()
                            .Window(handle)
                            .Url.ToLower()
                            .Contains(PageUrl.ToLower()))
                    {
                        loaded = true;
                    }
                }
            }
            catch (NullReferenceException nrEx)
            {
                //log infomation (
                //    "Window",
                //    this.PageUrl + " has closed during check, switching to Default window. " + nrEx.Message);
                DriverManager.Driver.SwitchTo().Window(handles[0]); // Switch to the default window
            }

            return loaded;
        }

        /// <summary>
        /// Select control, select by text helper
        /// </summary>
        /// <param name="selectLocator"></param>
        /// <param name="selectionText"></param>
        public void SelectControlHelper(By selectLocator, string selectionText)
        {
            var dropdown = DriverManager.FindWebElement(selectLocator);

            var selection = new SelectElement(dropdown);

            selection.SelectByText(selectionText);
        }
    }
}
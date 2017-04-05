namespace CTM.TestAutomation.Adapter.Energy.PageObjects
{
    using System;
    using System.Threading;

    using Core;
    using Core.ExtensionHelpers;

    using OpenQA.Selenium;

    /// <summary>
    /// If the test suite grew larger it probaly makes sense to inherit from some
    /// kind of app specific base page which may contain repeated controls/layouts across all pages
    /// </summary>
    public class YourSupplier : BasePage
    {
        #region Properties

        /// <summary>
        /// Static web elements/controls
        /// </summary>

        private static IWebElement InputPostCode => DriverManager.FindWebElement(By.Id("your-postcode"));

        private static IWebElement ButtonFindPostCode => DriverManager.FindWebElement(By.Id("find-postcode"));

        private static IWebElement InputHasBill => DriverManager.FindWebElement(By.CssSelector("#have-bill-label > span > span"));

        private static IWebElement InputNoBill => DriverManager.FindWebElement(By.CssSelector("#no-bill-label > span > span"));

        private static IWebElement InputGasAndElectricity => DriverManager.FindWebElement(By.CssSelector("#compare-both-label > span > span"));

        private static IWebElement InputElectricityOnly => DriverManager.FindWebElement(By.CssSelector("#compare-electricity-label > span > span"));

        private static IWebElement InputGasOnly => DriverManager.FindWebElement(By.CssSelector("#compare-gas-label > span > span"));

        private static IWebElement CheckBoxSameSupplyYes => DriverManager.FindWebElement(By.Id("same-supplier-yes"));

        private static IWebElement CheckBoxSameSupplyNo => DriverManager.FindWebElement(By.Id("same-supplier-no"));

        private static IWebElement ButtonNext => DriverManager.FindWebElement(By.Id("goto-your-supplier-details"));

        #endregion

        /// <summary>
        /// Constructor for Your Supplier Page Object
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="driver"></param>
        public YourSupplier(string baseUrl, DriverManager driver) : base(baseUrl, "?AFFCLIE=TSTT", driver)
        {
        }

        #region Public Methods

        /// <summary>
        /// Public test methods
        /// </summary>
        /// <param name="postcode"></param>

        public void FindPostCode(string postcode)
        {
            //in case the method gets called after a postcode has already been entered
            // if clear post code link present, click it
            InputPostCode.Clear();
            InputPostCode.SendKeys(postcode);
            ButtonFindPostCode.Click();
        }

        public bool WaitForResults()
        {
            return ResultsLoaded();
        }

        public void SelectIfUserHasBill(bool userHasBill)
        {
            // yes, I have bill is selected by default
            if (userHasBill) return;

            InputNoBill.Click();
        }

        public enum WhatDoYouWantToCompare
        {
            [EnumStringValue("Gas & Electricity")] GasAndElectric,
            [EnumStringValue("Electricity only")] ElectricOnly,
            [EnumStringValue("Gas only")] GasOnly
        }

        public void SelectWhatToCompare(WhatDoYouWantToCompare whatToCompare)
        {
            switch (whatToCompare)
            {
                case WhatDoYouWantToCompare.GasAndElectric:
                    InputGasAndElectricity.Click();
                    break;

                case WhatDoYouWantToCompare.ElectricOnly:
                    InputElectricityOnly.Click();
                    break;

                case WhatDoYouWantToCompare.GasOnly:
                    InputGasOnly.Click();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(whatToCompare), whatToCompare, null);
            }
        }

        public void CheckGasAndElectricSameSupplier(bool sameSupplier)
        {
            if (sameSupplier)
            {
                CheckBoxSameSupplyYes.Click();
            }
            else
            {
                CheckBoxSameSupplyNo.Click();
            }
        }

        public void SelectOneOfTopSixGasAndElectricity(string supplier)
        {
            const string wrapperText = "Who supplies your energy?";

            DriverManager.FindWebElement(TopSixSelector(wrapperText, supplier)).Click();
        }

        public void SelectOneOfTopSixGas(string supplier)
        {
            const string wrapperText = "Who supplies your gas?";

            DriverManager.FindWebElement(TopSixSelector(wrapperText, supplier)).Click();
        }

        public void SelectOneOfTopSixElectricity(string supplier)
        {
            const string wrapperText = "Who supplies your electricity?";

            DriverManager.FindWebElement(TopSixSelector(wrapperText, supplier)).Click();
        }

        public void ClickNext()
        {
            ButtonNext.Click();
        }

        #endregion

        #region Private Methods

        private static bool ResultsLoaded(int maxWait = 60)
        {
            var elapsedSeconds = 0;

            while (DriverManager.FindWebElement(By.Id("yourSupplier")).GetAttribute("class").Equals("ng-hide"))
            {
                Thread.Sleep(1000);
                elapsedSeconds += 1;

                if (elapsedSeconds == maxWait)
                {
                    return false;
                }
            }

            return true;
        }

        private static By TopSixSelector(string questionWrapperText, string supplierName)
        {
            return
                By.XPath("//span[text()='" + questionWrapperText + "']/..//label[@supplier-name='" + supplierName + "']");
        }

        #endregion
    }
}
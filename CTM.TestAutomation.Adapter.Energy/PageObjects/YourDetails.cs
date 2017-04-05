namespace CTM.TestAutomation.Adapter.Energy.PageObjects
{
    using Core;
    using System;
    using OpenQA.Selenium;

    /// <summary>
    /// Why is this page called Your Details? It's about payment preferences? your details
    /// suggests name, address, telephone etc to me...
    /// </summary>
    public class YourDetails : BasePage
    {
        private static IWebElement InputEmail => DriverManager.FindWebElement(By.Id("Email"));

        private static IWebElement CheckBoxAgreeTerms => DriverManager.FindWebElement(By.Id("terms-label"));

        private static IWebElement ButtonGoToPrices => DriverManager.FindWebElement(By.Id("email-submit"));

        public YourDetails(string baseUrl, DriverManager driver) : base(baseUrl, "yourDetails?AFFCLIE=TSTT", driver)
        {

        }

        public enum Tarriff
        {
            Variable,
            Fixed,
            All
        }

        public void SelectTarriff(Tarriff tarriff)
        {
            switch (tarriff)
            {
                case Tarriff.Variable:
                    DriverManager.FindWebElement(By.XPath("//span[@class='icon variable-bill-1']")).Click();
                    break;
                case Tarriff.Fixed:
                    DriverManager.FindWebElement(By.XPath("//span[@class='icon fixed-rate-1']")).Click();
                    break;
                case Tarriff.All:
                    DriverManager.FindWebElement(By.XPath("//span[@class='icon tariff-all']")).Click();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tarriff), tarriff, null);
            }
        }

        public void SelectPaymentTypeMonthlyDirectDebit()
        {
            // css selector isn't great here... I think the xpath makes for easier reading :/
            //#payment-selection-question > div > label.payment-monthly.checked > span > span
            DriverManager.FindWebElement(By.XPath("//span[@class='icon annual-1']")).Click();
        }

        public void SendEmail(string email)
        {
            InputEmail.Clear();
            InputEmail.SendKeys(email);
        }

        public void CheckUnderstood()
        {
            if (!DriverManager.FindWebElement(By.Id("terms")).Selected)
            {
                CheckBoxAgreeTerms.Click();
            }
        }

        public void GoToPrices()
        {
            ButtonGoToPrices.Click();
        }
    }
}
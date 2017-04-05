namespace CTM.TestAutomation.Adapter.Energy.PageObjects
{
    using Core;

    using OpenQA.Selenium;

    public class YourResults : BasePage
    {
        public YourResults(string baseUrl, DriverManager driver) : base(baseUrl, "yourResults?AFFCLIE=TSTT", driver)
        {
        }

        public void WaitForResults()
        {
            DriverManager.WaitForElement(By.XPath("//div[@id='interstitial-overlay'][@style='display: none;']"), 10);
        }

        public string ReturnSaving()
        {
            return
                DriverManager.FindWebElement(By.XPath("//div[contains(@id,'savingamount_cheapest')]"))
                    .Text.TrimStart('£');
        }

        public bool CheckCheapestSaving(int expectedMinimum)
        {
            var resultText = DriverManager.FindWebElement(By.XPath("//div[contains(@id,'savingamount_cheapest')]")).Text;

            var result = int.Parse(resultText);

            return result >= expectedMinimum;
        }
    }
}
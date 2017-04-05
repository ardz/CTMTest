namespace CTM.TestAutomation.Adapter.Energy.PageObjects
{
    using OpenQA.Selenium;

    using Core;

    public class YourEnergy : BasePage
    {
        private static IWebElement InputCurrentElectricitySpend
            => DriverManager.FindWebElement(By.Id("electricity-current-spend"));

        private static IWebElement InputCurrentGasSpend => DriverManager.FindWebElement(By.Id("gas-current-spend"));

        public YourEnergy(string baseUrl, DriverManager driver) : base(baseUrl, "yourEnergy?AFFCLIE=TSTT", driver)
        {
        }

        #region Layout When No Bill Present

        public void SendElectricitySpendInPounds(string amount)
        {
            InputCurrentElectricitySpend.Clear();
            InputCurrentElectricitySpend.SendKeys(amount);
        }

        public void SendGasSpendInPounds(string amount)
        {
            InputCurrentGasSpend.Clear();
            InputCurrentGasSpend.SendKeys(amount);
        }

        public void SelectSpendPeriodElectricity(string spendPeriod)
        {
            SelectControlHelper(By.Id("electricity-current-spend-period"), spendPeriod);
        }

        public void SelectSpendPeriodGas(string spendPeriod)
        {
            SelectControlHelper(By.Id("gas-current-spend-period"), spendPeriod);
        }

        #endregion
    }
}
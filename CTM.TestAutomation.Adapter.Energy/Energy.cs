namespace CTM.TestAutomation.Adapter.Energy
{
    using TechTalk.SpecFlow;

    using PageObjects;
    using Core;
    using Core.ExtensionHelpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System.Collections.Generic;
    
    using OpenQA.Selenium;

    /// <summary>
    /// Binding class for the energy switch journey. Contains step methods
    /// with Gherkin bindings which feature files can utilise.
    /// </summary>
    [Binding]
    public class Energy
    {
        private static DriverManager _driver;

        private static Dictionary<string, int> _resultsTable; 

        /// <summary>
        /// Wrapper class for all pages associated
        /// with the energy journey
        /// </summary>
        public Wrapper EnergyPageObjects;

        public DriverManager DriverManager
        {
            get { return _driver; }
            private set { _driver = value; }
        }

        public Energy()
        {
            if (this.DriverManager == null)
            {
                this.DriverManager = new DriverManager(TestManager.BrowserType, 60);
            }

            this.EnergyPageObjects = new Wrapper(TestManager.AutBaseUrl, DriverManager);
        }

        [Given(@"I am on the Energy Journey start page")]
        public void GoToStartPage()
        {
            EnergyPageObjects.
                YourSupplier().
                NavigateTo();
        }

        [When(@"I search for the post code (.+)")]
        public void StepEnterPostCode(string postCode)
        {
            EnergyPageObjects.
                YourSupplier().
                FindPostCode(postCode);
        }

        [When(@"I do have a bill")]
        public void StepUserHasBill()
        {
            EnergyPageObjects.
                YourSupplier().
                SelectIfUserHasBill(true);
        }

        [When(@"I do not have a bill")]
        public void StepUserDoesNotHaveBill()
        {
            EnergyPageObjects.
                YourSupplier().
                SelectIfUserHasBill(false);
        }

        [When(@"I select that my electricity is currently supplied by (.+)")]
        public void WhenISelectThatMyElectricityIsCurrentlySuppliedBy(string currentElectricity)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I select that my gas is currently supplied by (.+)")]
        public void WhenISelectThatMyGasIsCurrentlySuppliedBy(string currentGas)
        {
            ScenarioContext.Current.Pending();
        }


        [When(@"I select that I want to compare (GasAndElectric|GasOnly|ElectricOnly)")]
        public void StepSelectWhatToCompare(YourSupplier.WhatDoYouWantToCompare whatToCompare)
        {
            EnergyPageObjects.
                YourSupplier().
                SelectWhatToCompare(whatToCompare);
        }

        [Then(@"I should I see options for my current energy supplier")]
        public void WaitForResults()
        {
            Assert.IsTrue(EnergyPageObjects.YourSupplier().WaitForResults());
        }

        /// <summary>
        /// this below could be broken down into smaller methods
        /// for greater re-use across the test framework (or the table
        /// could be expanded to contain extra infomation such as the
        /// tariff the user wishes to switch to etc)
        /// </summary>
        /// <param name="table"></param>
        [Given(@"These user profiles where user has no bill and same provider for gas and electric:")]
        public void GivenTheseUserProfiles(Table table)
        {
            _resultsTable = new Dictionary<string, int>();

            var yourSupply = EnergyPageObjects.YourSupplier();

            yourSupply.NavigateTo();

            yourSupply.FindPostCode("PE2 6YS");

            yourSupply.WaitForResults();

            foreach (var profile in table.Rows)
            {
                // could also use dependancy injection and create a POCO
                // for each row in the table...

                yourSupply.SelectIfUserHasBill(false);

                yourSupply.SelectOneOfTopSixElectricity(profile["CurrentSupplier"]);

                yourSupply.SelectOneOfTopSixGas(profile["CurrentSupplier"]);

                ClickNextButton();

                var yourEnergy = EnergyPageObjects.YourEnergy();
                
                yourEnergy.SendElectricitySpendInPounds(profile["CurrentElectricitySpend"]);

                yourEnergy.SelectSpendPeriodElectricity(profile["Period"]);

                yourEnergy.SendGasSpendInPounds(profile["CurrentGasSpend"]);

                yourEnergy.SelectSpendPeriodGas(profile["Period"]);

                ClickNextButton();

                var yourDetails = EnergyPageObjects.YourDetails();

                yourDetails.SelectTarriff(YourDetails.Tarriff.Fixed);

                yourDetails.SelectPaymentTypeMonthlyDirectDebit();

                yourDetails.SendEmail(StringHelper.CreateRandomEmail());

                yourDetails.CheckUnderstood();

                yourDetails.GoToPrices();

                var yourResults = EnergyPageObjects.YourResults();

                yourResults.WaitForResults();

                _resultsTable.Add(profile["Profile"], int.Parse(yourResults.ReturnSaving()));

                yourSupply.NavigateTo();
            }
        }

        [Then(@"The minimum expected saving should be:")]
        public void ThenTheMinimumExpectedSavingShouldBe(Table saving)
        {
            foreach (var result in saving.Rows)
            {
                var expectedMinimumSaving = int.Parse(result["MinimumSaving"]);
                var actualSaving = _resultsTable[result["Profile"]];
                Assert.IsTrue(actualSaving >= expectedMinimumSaving);
            }
        }


        [When(@"I click the next button")]
        public void ClickNextButton()
        {
            DriverManager.FindWebElement(By.XPath("//button[text()='Next']")).Click();
        }
    }
}
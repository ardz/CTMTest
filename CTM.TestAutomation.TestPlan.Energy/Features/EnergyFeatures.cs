namespace CTM.TestAutomation.TestPlan.Energy.Features
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Core;
    using Adapter.Energy;

    using TechTalk.SpecFlow;

    /// <summary>
    /// Would usually inherit from some kind of base test class
    /// in a large test framework
    /// </summary>
    [Binding]
    [TestClass]
    public class EnergyFeatures
    {
        private static Energy _energyTestAdapter;

        [BeforeFeature]
        public static void BeforeFeature()
        {
            TestManager.SetTestConfigurationFile("\\TestConfig\\TestConfig.xml");

            _energyTestAdapter = new Energy();

            // would probably enable this code if you need
            // to set a starting state for any features
            // in the test plan
            
            //_energyTestAdapter.
            //    EnergyPageObjects.
            //    YourSupplier().
            //    NavigateTo();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            _energyTestAdapter.DriverManager.Quit();
        }

        /// <summary>
        /// The before scenario.
        /// </summary>
        [BeforeScenario]
        public void BeforeScenario()
        {
            // TODO: implement logic that has to run before executing each scenario
        }

        /// <summary>
        /// The after scenario.
        /// </summary>
        [AfterScenario]
        public void AfterScenario()
        {
            // TODO: implement logic that has to run after executing each scenario
        }
    }
}
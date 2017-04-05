namespace CTM.TestAutomation.Adapter.Energy.TestObjects
{
    /// <summary>
    /// May be useful to have an object which contains
    /// test data for an energy user profile, could then
    /// be shared between test steps
    /// </summary>
    public class EnergyUserProfile
    {
        public string Id;
        public string CurrentSupplier;
        public int CurrentElectricitySpend;
        public int CurrentGasSpend;
        public SpendUnit SpendUnit;
        public Period Period;
    }

    public enum SpendUnit
    {
        Pounds,
        Kwh
    }

    public enum Period
    {
        Month,
        Quarter,
        SixMonth,
        Annually
    }
}
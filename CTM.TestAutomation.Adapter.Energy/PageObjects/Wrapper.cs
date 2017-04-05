namespace CTM.TestAutomation.Adapter.Energy.PageObjects
{
    using Core;

    public class Wrapper
    {
        /// <summary>
        /// Gets the manager.
        /// </summary>
        private static DriverManager _driverManager;

        /// <summary>
        /// Gets or sets the base url.
        /// </summary>
        private static string _baseUrl;

        public Wrapper(string baseUrl, DriverManager driverManager)
        {
            _baseUrl = baseUrl;
            _driverManager = driverManager;
        }

        public YourDetails YourDetails()
        {
            return new YourDetails(_baseUrl, _driverManager);
        }

        public YourEnergy YourEnergy()
        {
            return new YourEnergy(_baseUrl, _driverManager);
        }

        public YourResults YourResults()
        {
            return new YourResults(_baseUrl, _driverManager);
        }

        public YourSupplier YourSupplier()
        {
            return new YourSupplier(_baseUrl, _driverManager);
        }
    }
}
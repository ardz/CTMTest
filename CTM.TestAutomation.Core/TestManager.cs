namespace CTM.TestAutomation.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <summary>
    /// TODO add some kind of test run logging framework
    /// </summary>
    public static class TestManager
    {
        /// <summary>
        /// The browser being used for the automated test
        /// </summary>
        public static BrowserTypes BrowserType { get; private set; }

        /// <summary>
        /// Base Url of the application/site under test
        /// </summary>
        public static string AutBaseUrl { get; private set; }

        /// <summary>
        /// The path of the config file being used for the tests
        /// </summary>
        public static string TestConfigFilePath { get; private set; }

        /// <summary>
        /// Holds all the settings keys and values from the test config xml file
        /// </summary>
        public static Dictionary<string, string> TestSettings { get; private set; }

        /// <summary>
        /// Returns the location of the currently executing assembly,
        /// used to find the Test Config file
        /// </summary>
        public static string GetBuildPath()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Sets the browser type the test is using
        /// </summary>
        private static void SetBrowserType()
        {
            var value = GetTestSettingValue("BrowserType");

            switch (value)
            {
                case "Firefox":
                {
                    BrowserType = BrowserTypes.Firefox;
                }

                    break;

                case "Chrome":
                {
                    BrowserType = BrowserTypes.Chrome;
                }

                    break;

                case "IE":
                {
                    BrowserType = BrowserTypes.Ie;
                }

                    break;

                case "Edge":
                {
                    BrowserType = BrowserTypes.Edge;
                }

                    break;

                default:
                {
                    BrowserType = BrowserTypes.Firefox;
                }

                    break;
            }
        }

        /// <summary>
        /// Sets the test config file to use with the test plan
        /// Call it from the test class setup
        /// </summary>
        /// <param name="file"></param>
        public static void SetTestConfigurationFile(string file)
        {
            TestConfigFilePath = GetBuildPath() + file;

            SetTestSettings();

            SetBrowserType();

            AutBaseUrl = GetTestSettingValue("AutBaseUrl");
        }

        /// <summary>
        /// Get's a key value from the test settings dictionary
        /// </summary>
        /// <param name="settingKey"></param>
        /// <returns>string containing the value</returns>
        public static string GetTestSettingValue(string settingKey)
        {
            string key;

            TestSettings.TryGetValue(settingKey, out key);

            return key;
        }

        /// <summary>
        /// assigns the settings from the xml file
        /// to the test settings dictionary
        /// </summary>
        private static void SetTestSettings()
        {
            var config = XDocument.Load(TestConfigFilePath);

            var xmlElement = config.Root?.Element("testSettings");

            if (xmlElement != null)
                TestSettings = xmlElement.Elements("add")
                    .ToDictionary(section => section.Attribute("key").Value, section => section.Attribute("value").Value);
        }
    }
}

namespace CTM.TestAutomation.Core.ExtensionHelpers
{
    using System;
    using System.Linq;
    using System.Text;

    public static class StringHelper
    {
        /// <summary>
        /// The random.
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// The sync lock.
        /// </summary>
        private static readonly object SyncLock = new object();

        /// <summary>
        /// creates a random alphanumeric string
        /// </summary>
        /// <param name="length">length of the string</param>
        /// <returns>random string</returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            // kept getting the same random string
            // http://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number
            // fix below?
            lock (SyncLock)
            {
                return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
            }
        }

        public static string CreateRandomString()
        {
            var rnd = new Random();

            var lenghtOfString = rnd.Next(5, 9);

            var randomString = new StringBuilder();

            for (var i = 0; i < lenghtOfString; i++)
            {
                var randomLetter = (char)('a' + rnd.Next(0, 26));
                randomString.Append(randomLetter);
            }
            return randomString.ToString();
        }

        public static string CreateRandomEmail()
        {
            var forename = CreateRandomString();

            var surname = CreateRandomString();

            var email = forename + surname + "@mail.com";

            return email;
        }

        public static string CreateRandomMobileNumber()
        {
            var rnd = new Random();

            var mobileNumber = "07";

            var i = 0;

            do
            {
                var digit = rnd.Next(0, 9).ToString();

                mobileNumber = mobileNumber + digit;

                i++;
            }
            while (i < 9);

            return mobileNumber;
        }

        public static string CreateRandomHomeTelephone()
        {
            var rnd = new Random();

            var homeTelephone = "0191 ";

            var i = 0;

            do
            {
                var digit = rnd.Next(0, 9).ToString();

                homeTelephone = homeTelephone + digit;

                i++;
            }
            while (i < 7);

            return homeTelephone;
        }
    }
}
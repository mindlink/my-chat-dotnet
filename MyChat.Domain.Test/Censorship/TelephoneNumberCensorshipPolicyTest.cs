namespace MindLink.Recruitment.MyChat.Domain.Test.Sensorship
{
    using Censorship;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class TelephoneNumberCensorshipPolicyTest
    {
        const string REPLACEMENT = "**retracted**";

        [TestMethod]
        public void It_can_censor_telephone_numbers_of_supported_countries()
        {
            List<string> telephoneNumbers = new List<string>();

            // United Kingdom
            foreach(string prefix in new string[] { "0", "0044", "+44" })
            {
                telephoneNumbers.Add(prefix + "1"  + string.Join("", Enumerable.Repeat(0, 9)));
                telephoneNumbers.Add(prefix + "2"  + string.Join("", Enumerable.Repeat(0, 9)));
                telephoneNumbers.Add(prefix + "3"  + string.Join("", Enumerable.Repeat(0, 9)));
                telephoneNumbers.Add(prefix + "55" + string.Join("", Enumerable.Repeat(0, 8)));
                telephoneNumbers.Add(prefix + "56" + string.Join("", Enumerable.Repeat(0, 8)));
                telephoneNumbers.Add(prefix + "7"  + string.Join("", Enumerable.Repeat(0, 9)));
                telephoneNumbers.Add(prefix + "8"  + string.Join("", Enumerable.Repeat(0, 9)));
                telephoneNumbers.Add(prefix + "1"  + string.Join("", Enumerable.Repeat(0, 8)));
                telephoneNumbers.Add(prefix + "5"  + string.Join("", Enumerable.Repeat(0, 8)));
                telephoneNumbers.Add(prefix + "8"  + string.Join("", Enumerable.Repeat(0, 8)));
                telephoneNumbers.Add(prefix + "8001111");
                telephoneNumbers.Add(prefix + "845464" + "1");
            }

            // Cyprus
            foreach(string prefix in new string[] { "", "00357", "+357" })
            {
                telephoneNumbers.Add(prefix + "22" + string.Join("", Enumerable.Repeat(0, 6)));
                telephoneNumbers.Add(prefix + "23" + string.Join("", Enumerable.Repeat(0, 6)));
                telephoneNumbers.Add(prefix + "24" + string.Join("", Enumerable.Repeat(0, 6)));
                telephoneNumbers.Add(prefix + "25" + string.Join("", Enumerable.Repeat(0, 6)));
                telephoneNumbers.Add(prefix + "26" + string.Join("", Enumerable.Repeat(0, 6)));
                telephoneNumbers.Add(prefix + "94" + string.Join("", Enumerable.Repeat(0, 6)));
                telephoneNumbers.Add(prefix + "95" + string.Join("", Enumerable.Repeat(0, 6)));
                telephoneNumbers.Add(prefix + "96" + string.Join("", Enumerable.Repeat(0, 6)));
                telephoneNumbers.Add(prefix + "97" + string.Join("", Enumerable.Repeat(0, 6)));
                telephoneNumbers.Add(prefix + "99" + string.Join("", Enumerable.Repeat(0, 6)));
            }

            var policy = new TelephoneNumberCensorshipPolicy(REPLACEMENT);
            foreach (var number in telephoneNumbers)
                Assert.AreEqual(REPLACEMENT, policy.Censor(number));
        }
    }
}

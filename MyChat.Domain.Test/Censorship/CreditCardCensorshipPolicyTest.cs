namespace MindLink.Recruitment.MyChat.Domain.Test.Sensorship
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Collections.Generic;
    using Censorship;

    [TestClass]
    public class CreditCardCensorshipPolicyTest
    {
        const string REPLACEMENT = "**retracted**";

        [TestMethod]
        public void It_can_censor_credit_card_numbers_of_supported_issuers()
        {

            var policy = new CreditCardCensorshipPolicy(REPLACEMENT);

            var creditCards = new List<string>();

            // American Express
            creditCards.Add("34" + string.Join(string.Empty, Enumerable.Repeat("1", 13)));
            creditCards.Add("37" + string.Join(string.Empty, Enumerable.Repeat("1", 13)));

            // Maestro
            creditCards.Add("50" + string.Join(string.Empty, Enumerable.Repeat("1", 10)));
            creditCards.Add("50" + string.Join(string.Empty, Enumerable.Repeat("1", 17)));
            creditCards.Add("56" + string.Join(string.Empty, Enumerable.Repeat("1", 10)));
            creditCards.Add("56" + string.Join(string.Empty, Enumerable.Repeat("1", 17)));
            creditCards.Add("69" + string.Join(string.Empty, Enumerable.Repeat("1", 10)));
            creditCards.Add("69" + string.Join(string.Empty, Enumerable.Repeat("1", 17)));

            // MasterCard
            creditCards.Add("2221" + string.Join(string.Empty, Enumerable.Repeat("1", 12)));
            creditCards.Add("2720" + string.Join(string.Empty, Enumerable.Repeat("1", 12)));
            creditCards.Add("5" + string.Join(string.Empty, Enumerable.Repeat("1", 15)));

            // Solo
            creditCards.Add("6334" + string.Join(string.Empty, Enumerable.Repeat("1", 12)));
            creditCards.Add("6334" + string.Join(string.Empty, Enumerable.Repeat("1", 14)));
            creditCards.Add("6334" + string.Join(string.Empty, Enumerable.Repeat("1", 15)));
            creditCards.Add("6767" + string.Join(string.Empty, Enumerable.Repeat("1", 12)));
            creditCards.Add("6767" + string.Join(string.Empty, Enumerable.Repeat("1", 14)));
            creditCards.Add("6767" + string.Join(string.Empty, Enumerable.Repeat("1", 15)));

            // Visa
            creditCards.Add("4" + string.Join(string.Empty, Enumerable.Repeat("1", 12)));
            creditCards.Add("4" + string.Join(string.Empty, Enumerable.Repeat("1", 15)));
            creditCards.Add("4" + string.Join(string.Empty, Enumerable.Repeat("1", 18)));

            foreach (string creditCard in creditCards)
                Assert.AreEqual(REPLACEMENT, policy.Censor(creditCard));
        }

        [TestMethod]
        public void It_skips_numbers_which_are_not_supported()
        {
            var policy = new CreditCardCensorshipPolicy(REPLACEMENT);

            var creditCards = new List<string>();

            // Diners Club International
            creditCards.Add("300" + string.Join(string.Empty, Enumerable.Repeat(1, 11)));

            foreach (string creditCard in creditCards)
                Assert.AreEqual(creditCard, policy.Censor(creditCard));
        }
    }
}

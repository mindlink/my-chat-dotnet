namespace MindLink.Recruitment.MyChat.Domain.Censorship
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public sealed class CreditCardCensorshipPolicy : ICensorshipPolicy
    {
        #region Helper Classes

        private sealed class CreditCardIssuerInfo
        {
            /// <summary>
            /// The Issuer Network
            /// </summary>
            public string Issuer { get; set; }

            /// <summary>
            /// The credit card number rules of the issuer.
            /// </summary>
            public IEnumerable<CreditCardNumberRule> Rules { get; set; }
        }

        private sealed class CreditCardNumberRule
        {
            /// <summary>
            /// The IIN prefixes of the cards issued by the network.
            /// </summary>
            public IEnumerable<string> Prefixes { get; set; }


            /// <summary>
            /// The lengths of the credit card numbers issued by the network.
            /// </summary>
            public IEnumerable<int> Lengths { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerable<Tuple<string, int>> PrefixLengthPairs
            {
                get
                {
                    return Prefixes.SelectMany(p => Lengths.Select(l => Tuple.Create(p, l)));
                }
            }
        }

        #endregion Helper Classes

        #region Static

        /// <summary>
        /// The regular expression pattern which can match all the supported credit card numbers.
        /// </summary>
        private static readonly string CreditCardRegexPattern;

        /// <summary>
        /// Initializes the static data when the class is first loaded.
        /// </summary>
        static CreditCardCensorshipPolicy()
        {
            IEnumerable<CreditCardIssuerInfo> info = GenerateCreditCardIssuersInfo();
            CreditCardRegexPattern = GenerateCreditCardRegexPattern(info);
        }

        /// <summary>
        /// Generates the data which describe the rules for the credit card numbers of each
        /// supported issuer.
        /// </summary>
        /// <returns>The </returns>
        private static IEnumerable<CreditCardIssuerInfo> GenerateCreditCardIssuersInfo()
        {
            // Partially reconstructed table found at https://en.wikipedia.org/wiki/Payment_card_number
            return new List<CreditCardIssuerInfo>()
            {
                new CreditCardIssuerInfo {
                    Issuer = "American Express",
                    Rules = new CreditCardNumberRule[] {
                        new CreditCardNumberRule {
                            Prefixes = new string[] { "34", "37" },
                            Lengths = new int[] { 15 }
                        }
                    }
                },
                new CreditCardIssuerInfo {
                    Issuer = "Maestro",
                    Rules = new CreditCardNumberRule[] {
                        new CreditCardNumberRule {
                            Prefixes = new string[] { "50" }.Union(Enumerable.Range(56, 14).Select(i => i.ToString())),
                            Lengths = Enumerable.Range(12, 8)
                        }
                    }
                },
                new CreditCardIssuerInfo {
                    Issuer ="MasterCard",
                    Rules = new CreditCardNumberRule[] {
                        new CreditCardNumberRule {
                            Prefixes = Enumerable.Range(2221, 500).Select(i => i.ToString()),
                            Lengths = new int[] { 16 }
                        },
                        new CreditCardNumberRule {
                            Prefixes = new string[] { "5" },
                            Lengths = new int[] { 16 }
                        }
                    }
                },
                new CreditCardIssuerInfo {
                    Issuer = "Solo",
                    Rules = new CreditCardNumberRule[] {
                        new CreditCardNumberRule {
                            Prefixes = new string[] { "6334", "6767" },
                            Lengths = new int[] { 16, 18, 19 }
                        }
                    }
                },
                new CreditCardIssuerInfo {
                    Issuer = "Visa",
                    Rules = new CreditCardNumberRule[] {
                        new CreditCardNumberRule {
                            Prefixes = new string[] { "4" },
                            Lengths = new int[] { 13, 16, 19 }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Given a collection with the rules for generating the credit card numbers of several
        /// issuers it generates the regular expression pattern which can match these numbers.
        /// </summary>
        /// <param name="issuers"></param>
        /// <returns>The regular expression pattern which can match the credit card numbers from all the specified rules.</returns>
        private static string GenerateCreditCardRegexPattern(IEnumerable<CreditCardIssuerInfo> issuers)
        {
            IEnumerable<Tuple<string, int>> prefixLengthPairs = issuers
                .SelectMany(i => i.Rules)
                .SelectMany(r => r.PrefixLengthPairs);

            // Notes:
            // 1. We do not match at word boundaries since a credit card is considered to be sensitive
            //    information. This way, although we can have false positive matches,
            //    we do not have false negatives.
            // 2. Since we do not anchor each credit card rule at word boundaries, two rules
            //    with the same prefix and different lengths can match the same partial
            //    credit card number. We sort the list of rules in descenting order based on
            //    the total length of the expressed number in order to give priority to rules
            //    which match the most digits.
            IEnumerable<string> subPatterns = prefixLengthPairs
                .OrderByDescending(p => p.Item1.Length + p.Item2)
                .Select(p => $"{p.Item1}\\d{{{p.Item2 - p.Item1.Length}}}");

            return $"(?<!\\d)({string.Join("|", subPatterns)})(?!\\d)";
        }

        #endregion Static

        private readonly string _replacement;

        /// <summary>
        /// Initializes a <see cref="CreditCardCensorshipPolicy"/>
        /// </summary>
        /// <param name="replacement">The phrase to replace each credit card</param>
        public CreditCardCensorshipPolicy(string replacement)
        {
            if (replacement == null)
                throw new ArgumentNullException($"The value of {nameof(replacement)} cannot be null.");

            _replacement = replacement;
        }

        /// <summary>
        /// Censors the specified value.
        /// </summary>
        /// <param name="value">The value to censor.</param>
        /// <returns></returns>
        public string Censor(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // Note: For our purposes, Regex matching seems to be a good enough solution.
            //       However, we can make the matching logic more robust by additionally
            //       performing Luhn validation. Keep in mind that not every issuer
            //       supports it.

            var regex = new Regex(CreditCardRegexPattern);
            return regex.Replace(value, _replacement);
        }
    }
}

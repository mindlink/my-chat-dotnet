namespace MindLink.Recruitment.MyChat.Domain.Censorship
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class TelephoneNumberCensorshipPolicy : ICensorshipPolicy
    {
        #region Helper Types

        private sealed class CountryInfo
        {
            /// <summary>
            /// The name of the country
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The country code.
            /// </summary>
            public string CountryCode { get; set; }

            /// <summary>
            /// The rules for the possible telephone numbers.
            /// </summary>
            public IEnumerable<TelephoneNumberRule> Rules { get; set; }
        }

        private sealed class TelephoneNumberRule
        {
            /// <summary>
            /// The trunk prefixes
            /// </summary>
            public string TrunkPrefix { get; set; }

            /// <summary>
            /// The prefix of the telephone number.
            /// </summary>
            public string Prefix { get; set; }

            /// <summary>
            /// The total length of the telephone number without the <see cref="TrunkPrefix"/>.
            /// </summary>
            public int Length { get; set; }
        }

        #endregion Helper Types

        #region Static

        /// <summary>
        /// The regular expression pattern which can match the supported phone numbers.
        /// </summary>
        private static readonly string PhoneNumberRegexPattern;

        /// <summary>
        /// Initializes the class when it is loaded for the first time.
        /// </summary>
        static TelephoneNumberCensorshipPolicy()
        {
            IEnumerable<CountryInfo> countryData = GenerateCountryInfoData();
            PhoneNumberRegexPattern = GeneratePhoneNumberRegexPattern(countryData);
        }

        /// <summary>
        /// Generates the data which describes the format of the telephone numbers for each supported country.
        /// </summary>
        /// <returns>The list of telephone number data for each supported country.</returns>
        private static IEnumerable<CountryInfo> GenerateCountryInfoData()
        {
            return new List<CountryInfo>
            {
                // Reconstructed based on the table at https://en.wikipedia.org/wiki/Telephone_numbers_in_the_United_Kingdom
                new CountryInfo
                {
                    Name = "United Kingdom",
                    CountryCode = "44",
                    Rules = new List<TelephoneNumberRule>
                    {
                        new TelephoneNumberRule { Prefix = "1"      , Length = 10, TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "2"      , Length = 10, TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "3"      , Length = 10, TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "55"     , Length = 10, TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "56"     , Length = 10, TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "7"      , Length = 10, TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "8"      , Length = 10, TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "9"      , Length = 9,  TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "1"      , Length = 9,  TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "5"      , Length = 9,  TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "8"      , Length = 9,  TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "8001111", Length = 7,  TrunkPrefix = "0" },
                        new TelephoneNumberRule { Prefix = "845464" , Length = 7,  TrunkPrefix = "0" },
                    }
                },
                new CountryInfo
                {
                    Name = "Cyprus",
                    CountryCode = "357",
                    Rules = new List<TelephoneNumberRule>
                    {
                        new TelephoneNumberRule { Prefix = "22", Length = 8 },
                        new TelephoneNumberRule { Prefix = "23", Length = 8 },
                        new TelephoneNumberRule { Prefix = "24", Length = 8 },
                        new TelephoneNumberRule { Prefix = "25", Length = 8 },
                        new TelephoneNumberRule { Prefix = "26", Length = 8 },
                        new TelephoneNumberRule { Prefix = "94", Length = 8 },
                        new TelephoneNumberRule { Prefix = "95", Length = 8 },
                        new TelephoneNumberRule { Prefix = "96", Length = 8 },
                        new TelephoneNumberRule { Prefix = "97", Length = 8 },
                        new TelephoneNumberRule { Prefix = "99", Length = 8 },
                    }
                }
            };
        }

        /// <summary>
        /// Given a list with the telephone number data of each country, it generates the regular
        /// expression pattern which can match the supported telephone numbers.
        /// </summary>
        /// <param name="countryInfo"></param>
        /// <returns></returns>
        private static string GeneratePhoneNumberRegexPattern(IEnumerable<CountryInfo> countryInfo)
        {
            List<string> patterns = new List<string>();

            foreach (var c in countryInfo)
            {
                foreach (var r in c.Rules)
                {
                    // domestic calls
                    if (string.IsNullOrWhiteSpace(r.TrunkPrefix))
                        patterns.Add($"(?<!\\d){r.Prefix}\\d{{{r.Length - r.Prefix.Length}}}(?!\\d)");
                    else
                        patterns.Add($"(?<!\\d){r.TrunkPrefix}{r.Prefix}\\d{{{r.Length - r.Prefix.Length}}}(?!\\d)");

                    // international calls
                    patterns.Add($"([\\+]|(?<!\\d)00){c.CountryCode}{r.Prefix}\\d{{{r.Length - r.Prefix.Length}}}(?!\\d)");
                }
            }

            return string.Join("|", patterns);
        }

        #endregion Static

        private readonly string _replacement;

        /// <summary>
        /// Initializes a <see cref="CreditCardCensorshipPolicy"/>
        /// </summary>
        /// <param name="replacement">The phrase to replace each credit card</param>
        public TelephoneNumberCensorshipPolicy(string replacement)
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

            var regex = new Regex(PhoneNumberRegexPattern);
            return regex.Replace(value, _replacement);
        }
    }
}

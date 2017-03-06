namespace MyChatLibrary
{
    using System;

    /// <summary>
    /// Represents the configuration for the exporter.
    /// </summary>
    public sealed class ConversationExporterConfiguration
    {
        /// <summary>
        /// The input file path.
        /// </summary>
        public string inputFilePath;

        /// <summary>
        /// The output file path.
        /// </summary>
        public string outputFilePath;

        /// <summary>
        /// The USER Filter
        /// </summary>
        public string userFilter;

        /// <summary>
        /// The KEYWORD Including Filter
        /// </summary>
        public string keywordInclude;

        /// <summary>
        /// The KEYWORD Excluding Filter
        /// </summary>
        public string keywordExclude;

        /// <summary>
        /// The BLACKLIST Filterlist
        /// </summary>
        public string blacklistFilter;

        /// <summary>
        /// The hide Credit Cards
        /// </summary>
        public bool hideCreditCard;

        /// <summary>
        /// The hide Phone Numbers
        /// </summary>
        public bool hidePhone;

        /// <summary>
        /// Obfuscate User Name
        /// </summary>
        public bool obfuscateUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class.
        /// </summary>
        /// <param name="inputFilePath">
        /// The input file path.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        public ConversationExporterConfiguration(string inputFilePath, string outputFilePath, string userFilter)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            this.userFilter = userFilter;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationExporterConfiguration"/> class with the given Input/Output Parameter position.
        /// </summary>
        /// <param name="imputArguments">
        /// Argument List.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any of the given arguments is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when any of the given arguments is empty.
        /// </exception>
        public ConversationExporterConfiguration(string[] inputArguments)
        {
            String[] _param = null;

            Console.WriteLine("Parameter\n--------");        // write title to console

            // Parameter 1 & 2 are fixed, parse the others in any sequence
            this.inputFilePath = inputArguments[0];
            this.outputFilePath = inputArguments[1];

            // Parameter parser
            foreach (var element in inputArguments)
            {
                // pick the parameter name, if it fails, take the next one
                try
                {
                    _param = element.Split('=');
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;       // no need for action or throw - ignore and next
                }

                switch (_param[0])
                {
                    // user handler
                    case "user":
                        {
                            this.userFilter = _param[1];
                            Console.WriteLine("User: " + this.userFilter);
                            break;
                        }

                    // keyword include handler
                    case "keyword_include":
                        {
                            this.keywordInclude = _param[1];
                            Console.WriteLine("Including Words: " + this.keywordInclude);
                            break;
                        }


                    // keyword exclude handler
                    case "keyword_exclude":
                        {
                            this.keywordExclude = _param[1];
                            Console.WriteLine("Excluding Words" + this.keywordExclude);
                            break;
                        }

                    // blacklist handler
                    case "blacklist":
                        {
                            this.blacklistFilter = _param[1];
                            Console.WriteLine("Blacklist: " + this.blacklistFilter);
                            break;
                        }

                    // Hide CreditCard handler
                    case "hideCreditCard":
                        {
                            this.hideCreditCard = true;
                            Console.WriteLine("Hide Credit Cards: " + this.hideCreditCard);
                            break;
                        }

                    // Hide Phone Number handler
                    case "hidePhone":
                        {
                            this.hidePhone = true;
                            Console.WriteLine("Hide Phone Numer: " + this.hidePhone);
                            break;
                        }

                    // Obfuscate Username
                    case "obfuscateUser":
                        {
                            this.obfuscateUser = true;
                            Console.WriteLine("Obfuscate User = " + this.obfuscateUser);
                            break;
                        }
                }
            }

            Console.WriteLine("\n");
        }
    }
}

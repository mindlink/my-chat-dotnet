namespace MindLink.Recruitment.MyChat.UI.Console.Options
{
    using Fclp;
    using System;

    /// <summary>
    /// Command Line parser for the application.
    /// </summary>
    public sealed class OptionsParser
    {
        private readonly Action<Options> _onSuccess;
        private readonly Action<string> _onError;
        private readonly Action<string> _onHelp;

        /// <summary>
        /// Initializes an <see cref="OptionsParser"/>
        /// </summary>
        /// <param name="onSuccess">Action which is called when the arguments are successfully parsed.</param>
        /// <param name="onError">Action which is called when there are errors in the arguments.</param>
        /// <param name="onHelp">Action which is called when the the 'help' option is specified.</param>
        public OptionsParser(Action<Options> onSuccess, Action<string> onError, Action<string> onHelp)
        {
            if (onSuccess == null)
                throw new ArgumentNullException($"{nameof(onSuccess)}");

            if (onError == null)
                throw new ArgumentNullException($"{nameof(onError)}");

            if (onHelp == null)
                throw new ArgumentNullException($"{nameof(onHelp)}");

            _onSuccess = onSuccess;
            _onError = onError;
            _onHelp = onHelp;
        }

        /// <summary>
        /// Parses the command line arguments and generates an instance of <see cref="Options"/>.
        /// </summary>
        /// <param name="arguments">The command line arguments</param>
        /// <returns>The generated <see cref="Options>" instance./></returns>
        public void Parse(string[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException($"{nameof(arguments)}");

            var parser = new FluentCommandLineParser<Options>();
            Setup(parser);

            ICommandLineParserResult result = parser.Parse(arguments);
            if (result.HelpCalled)
                return;

            if (!result.HasErrors)
                _onSuccess(parser.Object);
            else
                _onError(result.ErrorText);
        }

        /// <summary>
        /// Setup a <see cref="FluentCommandLineParser<Options>"/> parser.
        /// </summary>
        /// <param name="parser"></param>
        private void Setup(FluentCommandLineParser<Options> parser)
        {
            parser.Setup(o => o.InputFile)
                .As('i', "input")
                .Required()
                .WithDescription(Resources.InputFileOptionDescription);

            parser.Setup(o => o.OutputFile)
                .As('o', "output")
                .Required()
                .WithDescription(Resources.OutputFileOptionDescription);

            parser.Setup(o => o.UserFilter)
                .As("filter-by-user")
                .WithDescription(Resources.UserFilterOptionDescription);

            parser.Setup(o => o.ContentKeywordFilter)
                .As("filter-by-keyword")
                .WithDescription(Resources.ContentKeywordFilterOptionDescription);

            parser.Setup(o => o.KeywordsToCensor)
                .As("censor-keywords")
                .WithDescription(Resources.KeywordsToCensorOptionDescription);

            parser.Setup(o => o.CensorSensitiveInformation)
                .As("censor-sensitive-info")
                .WithDescription(Resources.CensorSensitiveInformationOptionDescription);

            parser.Setup(o => o.ObfuscateUser)
                .As("obfuscate-user")
                .WithDescription(Resources.ObfuscateUserOptionDescription);

            parser.Setup(o => o.GenerateMostActiveUsersReport)
                .As("activity-report")
                .WithDescription(Resources.GenerateMostActiveUsersReportOptionDescription);

            parser.SetupHelp("?", "help")
                .Callback(_onHelp)
                .WithCustomFormatter(new OptionsFormatter());
        }
    }
}

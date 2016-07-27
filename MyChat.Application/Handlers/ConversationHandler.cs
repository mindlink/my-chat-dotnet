namespace MindLink.Recruitment.MyChat.Application.Handlers
{
    using Commands;
    using Data;
    using Domain.Censorship;
    using Domain.Conversations;
    using Domain.Obfuscation;
    using Domain.Reporting;
    using Domain.Specifications;
    using Results;
    using Services;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;


    /// <summary>
    /// Handler of <see cref="ExportConversationCommand"/>
    /// </summary>
    public sealed class ConversationHandler: ICommandHandler<ExportConversationCommand, ExportConversationResult>
    {
        private IConversationReader _conversationReader;

        /// <summary>
        /// Initializes the <see cref="ConversationHandler"/>
        /// </summary>
        /// <param name="conversationReader">The conversationr reader to use in order to convert the input into a conversation.</param>
        public ConversationHandler(IConversationReader conversationReader)
        {
            _conversationReader = conversationReader;
        }

        /// <summary>
        /// Handles the <see cref="ExportConversationCommand"/>
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <returns>The result of the operation.</returns>
        public ExportConversationResult Handle(ExportConversationCommand command)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(command, new ValidationContext(command), validationResults))
                return ExportConversationResult.Failure(validationResults.Select(e => e.ErrorMessage));

            try
            {
                Encoding streamEncoding = command.StreamEncoding ?? Encoding.ASCII;

                ISpecification<Message> messageSpec = CreateMessageSpecification(
                    command.UserIdFilter,
                    command.ContentKeywordFilter);

                Conversation conversation = _conversationReader.Read(
                    new MemoryStream(command.Input),
                    streamEncoding,
                    messageSpec);

                ICensorshipPolicy contentCensorshipPolicy = CreateContentCensorshipPolicy(
                    command.KeywordsToCensor,
                    command.CensorCreditCardNumbers,
                    command.CensorTelephoneNumbers);

                IObfuscationPolicy userObfuscationPolicy = CreateUserObfuscationPolicy(
                    command.ObfuscateUserId);

                conversation.CensorMessages(contentCensorshipPolicy);
                conversation.ObfuscateSenders(userObfuscationPolicy);

                IEnumerable<UserActivity> mostActiveUsers = command.GenerateMostActiveUsersReport
                    ? GetMostActiveUsers(conversation)
                    : null;

                var dto = new ConversationDTO(conversation, mostActiveUsers);
                return ExportConversationResult.Success(dto);
            }
            catch (ConversationReaderErrorException)
            {
                return ExportConversationResult.Failure(new string[] { Resources.InvalidInputStreamErrorMessage });
            }
        }

        /// <summary>
        /// Creates a specification for messages of a conversation based on the specified criteria.
        /// </summary>
        /// <param name="userIdFilter">When specified, only messages of this used id satisfy the specification.</param>
        /// <param name="contentKeywordFilter">When specified, only message whose content contains the specified keyword satisfy the specification.</param>
        /// <returns>The specification.</returns>
        private ISpecification<Message> CreateMessageSpecification(string userIdFilter, string contentKeywordFilter)
        {
            List<ISpecification<Message>> specifications = new List<ISpecification<Message>>();

            if (!string.IsNullOrEmpty(userIdFilter))
                specifications.Add(new MessageOfUserSpecification(userIdFilter));

            if (!string.IsNullOrEmpty(contentKeywordFilter))
                specifications.Add(new MessageContainingKeywordSpecification(contentKeywordFilter));

            if (specifications.Any())
                return specifications.Aggregate((accSpec, nextSpec) => accSpec.And(nextSpec));

            return new EveryMessageSpecification();
        }

        /// <summary>
        /// Creates a censorship policy for messages of a conversation based on the specified criterial.
        /// </summary>
        /// <param name="keywordsToCensor">A list of keywords which must be censored.</param>
        /// <param name="censorCreditCardNumbers">When true, credit card numbers are censored.</param>
        /// <param name="censorTelephoneNumbers">When true, telephone numbers are censored.</param>
        /// <returns>The censorship policy.</returns>
        private ICensorshipPolicy CreateContentCensorshipPolicy(IEnumerable<string> keywordsToCensor, bool censorCreditCardNumbers, bool censorTelephoneNumbers)
        {
            var policy = new PipelineCensorshipPolicy();

            if (keywordsToCensor != null && keywordsToCensor.Any())
                policy.AddPolicy(new BlacklistCensorshipPolicy(keywordsToCensor, Resources.CensoredTextReplacement));

            if (censorCreditCardNumbers)
                policy.AddPolicy(new CreditCardCensorshipPolicy(Resources.CensoredTextReplacement));

            if (censorTelephoneNumbers)
                policy.AddPolicy(new TelephoneNumberCensorshipPolicy(Resources.CensoredTextReplacement));

            return policy;
        }

        /// <summary>
        /// Creates an obfuscation policy of the user id in a message.
        /// </summary>
        /// <param name="obfuscateUserId">When true, the user id is obfuscated.</param>
        /// <returns></returns>
        private IObfuscationPolicy CreateUserObfuscationPolicy(bool obfuscateUserId)
        {
            if (obfuscateUserId)
                return new ReversedBase64ObfuscationPolicy();
            return new NullObfuscationPolicy();
        }

        /// <summary>
        /// Returns the activity of each user in a conversation sorted by the most active user.
        /// </summary>
        /// <param name="conversation">The conversation</param>
        /// <returns>The list of user activities.</returns>
        private IEnumerable<UserActivity> GetMostActiveUsers(Conversation conversation)
        {
            var reportFactory = new ReportFactory();
            MostActiveUsersReport report = reportFactory.CreateMostActiveUsersReport(conversation);
            return report.Generate();
        }
    }

}

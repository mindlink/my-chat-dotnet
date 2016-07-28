namespace MindLink.Recruitment.MyChat.Application.Commands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Export conversation command.
    /// </summary>
    public sealed class ExportConversationCommand : IValidatableObject
    {
        /// <summary>
        /// The input bytes to process.
        /// </summary>
        [Required(ErrorMessage = "The Input Stream is required.")]
        public byte[] Input { get; set; }

        /// <summary>
        /// The encoding of the input stream. When not specified, ASCII encoding is assumed.
        /// </summary>
        public Encoding StreamEncoding { get; set; }

        /// <summary>
        /// When a value is present, it is used in order to filter the messages
        /// of the conversation which belong to that specific user.
        /// </summary>
        public string UserIdFilter { get; set; }

        /// <summary>
        /// When a value is present, it is used in order to filter the messages
        /// of the conversation which contain the specific keyword.
        /// </summary>
        public string ContentKeywordFilter { get; set; }

        /// <summary>
        /// When a collection is present, each value which is present in the
        /// content of the resulting messages is censored.
        /// </summary>
        public IEnumerable<string> KeywordsToCensor { get; set; }

        /// <summary>
        /// When true, any detected credit card number in the content of the 
        /// resulting messages is censored .
        /// </summary>
        public bool CensorCreditCardNumbers { get; set; }

        /// <summary>
        /// When true, any detected telephone numbers in the content of the
        /// resulting messages is censored.
        /// </summary>
        public bool CensorTelephoneNumbers { get; set; }

        /// <summary>
        /// When true, the id of each user in the resulting messages is obfuscated.
        /// </summary>
        public bool ObfuscateUserId { get; set; }

        /// <summary>
        /// When true, a report is generated with the most active users based on the
        /// messages in the conversation.
        /// </summary>
        public bool GenerateMostActiveUsersReport { get; set; }

        /// <summary>
        /// Performs additional validations for the request.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The validation results.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Input.Length == 0)
                yield return new ValidationResult("The input must not be empty.");
        }
    }
}

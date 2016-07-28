using MindLink.Recruitment.MyChat.Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Application.Results
{
    public sealed class ExportConversationResult
    {
        private readonly List<string> _errors;

        private ExportConversationResult(bool isSuccess, ConversationDTO conversation, IEnumerable<string> errors)
        {
            IsSuccess = isSuccess;
            Conversation = conversation;

            _errors = new List<string>();
            if (errors != null)
                _errors.AddRange(errors);
        }

        /// <summary>
        /// Indicates whether the result is successfull.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// The serialized result of the request.
        /// </summary>
        public ConversationDTO Conversation { get; private set; }

        /// <summary>
        /// The errors generated while processing the request.
        /// </summary>
        public IEnumerable<string> Errors
        {
            get { return _errors; }
        }

        /// <summary>
        /// Factory method of a <see cref="ExportConversationResult"/> which indicates a successfull operation which indicates a successfull operation.
        /// </summary>
        /// <param name="conversation">The exported conversation.</param>
        /// <returns></returns>
        public static ExportConversationResult Success(ConversationDTO conversation)
        {
            return new ExportConversationResult(true, conversation, null);
        }

        /// <summary>
        /// Factory method of a <see cref="ExportConversationResult"/> which indicates a successfull operation which indicates a failed operation.
        /// </summary>
        /// <param name="errors">The collection of errors produced by the command handler.</param>
        /// <returns></returns>
        public static ExportConversationResult Failure(IEnumerable<string> errors)
        {
            return new ExportConversationResult(false, null, errors);
        }
    }
}

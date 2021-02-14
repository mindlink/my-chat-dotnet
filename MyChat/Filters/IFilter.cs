using MindLink.Recruitment.MyChat.Data;

namespace MindLink.Recruitment.MyChat.Filters
{
    public interface IFilter
    {
        /// <summary>
        /// Filters the chat by a given input.
        /// </summary>
        /// <param name="conversation">
        /// The conversation to filter.
        /// </param>
        /// <returns>
        /// Returns a filtered conversation.
        /// </returns>
        Conversation Filter(Conversation conversation);
    }
}

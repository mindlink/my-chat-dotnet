namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public interface ICommandLineParser
    {
        /// <summary>
        /// Returns a custom <see cref="ConversationConfig"/> object defined by command line arguments
        /// </summary>
        /// <param name="arguments"></param>
        /// The command line arguments
        /// <returns></returns>
        ConversationConfig ParseCommandLineArguments(string[] arguments);
    }
}
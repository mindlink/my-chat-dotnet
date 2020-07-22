namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Used to filter <see cref="Message"/> objects.
    /// </summary>
    public interface IMessageFilter
    {
        /// <summary>
        /// Returns a filtered <see cref="Message"/> object.
        /// </summary>
        /// <param name="message">
        /// Message object to be filtered.
        /// </param>
        Message FilterMessage(Message message);
    }
}
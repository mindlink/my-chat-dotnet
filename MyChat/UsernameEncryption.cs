namespace MindLink.Recruitment.MyChat
{
    using System;

    /// <summary>
    /// Represents a helper class for username encryption within Conversation object.
    /// </summary>
    public static class UserNameEncryption
    {
        /// <summary>
        /// Encrypts the usernames in Conversation.
        /// </summary>
        /// <param name="conversation">The conversation to encrypt usernames in.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when conversation is null or the conversation messages is null.
        /// </exception>
        public static void EncryptUserNames(Conversation conversation)
        {
            if (conversation == null || conversation.Messages == null)
            {
                throw new ArgumentNullException("conversation", "Conversation cannot be null when trying to encrypt usernames.");
            }

            foreach(Message message in conversation.Messages)
            {
                message.SenderId = Encryption.Encrypt(message.SenderId);
            }
        }
    }
}

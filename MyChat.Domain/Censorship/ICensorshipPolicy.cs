namespace MindLink.Recruitment.MyChat.Domain.Censorship
{
    /// <summary>
    /// Interface of a censorhip policy for strings
    /// </summary>
    public interface ICensorshipPolicy
    {
        /// <summary>
        /// Censors the specified value.
        /// </summary>
        /// <param name="value">The value to censor.</param>
        /// <returns>The censored value.</returns>
        string Censor(string value);
    }
}

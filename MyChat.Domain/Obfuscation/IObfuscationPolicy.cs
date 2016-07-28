namespace MindLink.Recruitment.MyChat.Domain.Obfuscation
{
    /// <summary>
    /// Interface of an obfuscation policy for strings.
    /// </summary>
    public interface IObfuscationPolicy
    {
        /// <summary>
        /// Obfuscates the specified value.
        /// </summary>
        /// <param name="value">The value to obfuscate.</param>
        /// <returns>The obfuscated value.</returns>
        string Obfuscate(string value);
    }
}

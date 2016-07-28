namespace MindLink.Recruitment.MyChat.Domain.Obfuscation
{
    /// <summary>
    /// A null obfuscation policy which always returns the original value unchanged.
    /// It implements the 'Null Object Pattern' for <see cref="IObfuscationPolicy"/>
    /// </summary>
    public sealed class NullObfuscationPolicy : IObfuscationPolicy
    {
        public string Obfuscate(string value)
        {
            return value;
        }
    }
}

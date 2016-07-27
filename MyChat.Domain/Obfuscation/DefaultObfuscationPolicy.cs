namespace MindLink.Recruitment.MyChat.Domain.Obfuscation
{
    using Common.Extensions;
    using System;
    using System.Text;

    public class ReversedBase64ObfuscationPolicy: IObfuscationPolicy
    {
        /// <summary>
        /// Obfuscates the specified value by reversing the base16 representation of the value.
        /// </summary>
        /// <param name="value">The value to obfuscate.</param>
        /// <returns></returns>
        public string Obfuscate(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            string obfuscated = Convert.ToBase64String(valueBytes);
            return obfuscated.Reverse();
        }
    }
}

namespace MindLink.Recruitment.MyChat.UI.Console.Options
{
    using System.Collections.Generic;
    using Fclp;
    using Fclp.Internals;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Custom options formater
    /// </summary>
    public sealed class OptionsFormatter : ICommandLineOptionFormatter
    {
        public string Format(IEnumerable<ICommandLineOption> options)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();

            foreach (var option in options)
            {
                string optionsText = "";
                if (option.HasShortName && option.HasLongName)
                    optionsText = $"-{option.ShortName}, --{option.LongName}";
                else if (option.HasShortName)
                    optionsText = $"-{option.ShortName}";
                else
                    optionsText = $"--{option.LongName}";

                if (!string.IsNullOrWhiteSpace(option.Description))
                    sb.AppendLine($"\t{optionsText}:\n\t\t{option.Description}");
                else
                    sb.AppendLine($"\t{optionsText}:\n\n");
            }

            return sb.ToString();
        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents a helper to parse command line arguments.
    /// </summary>
    public sealed class CLAParser
    {
        /// <summary>
        /// Parses the given <paramref name="arguments"/> into the exporter configuration.
        /// </summary>
        /// <param name="arguments">
        /// The command line arguments.
        /// </param>
        /// <returns>
        /// A <see cref="ConversationExporterConfiguration"/> representing the command line arguments.
        /// </returns>
        ConversationExporterConfiguration configuration;

        public CLAParser()
        {
            configuration = new ConversationExporterConfiguration();
        }

        public ConversationExporterConfiguration ParseCommandLineArguments(string[] arguments)
        {
            if (arguments.Length < 2)
            {
                throw new IndexOutOfRangeException("2 or more parameters required.");
            }
            else if (arguments.Length >= 2 && arguments.Length <= 4)
            {
                configuration.InputFilePath = arguments[0];
                configuration.OutputFilePath = arguments[1];
                if (arguments.Length == 3)
                {
                    if (arguments[2] == "sensitive")
                    {
                        configuration.PersonalNumbers = true;
                    }
                    else
                    {
                        throw new ArgumentException("Cannot parse given argument");
                    }
                }
                if (arguments.Length == 4)
                {
                    // var split = arguments[2].ToLower().Split(":");
                    if (arguments[2] == "name")
                    {
                        configuration.FilterID = true;
                        configuration.Filter = arguments[3];
                    }
                    else if (arguments[2] == "word")
                    {
                        configuration.FilterID = false;
                        configuration.Filter = arguments[3];
                    }
                    else if (arguments[2] == "blacklist")
                    {
                        configuration.Blacklist = true;
                        try
                        {
                            using (var reader = new StreamReader(new FileStream(arguments[3], FileMode.Open, FileAccess.Read),
                                Encoding.ASCII))
                            {
                                var listOfWords = reader.ReadLine().Split(" ");
                                for (int i = 0; i < listOfWords.Length; i++)
                                {
                                    // Regex to catch all punctuation and replace with no space. This concatenates words such as "I'm"
                                    listOfWords[i] = Regex.Replace(listOfWords[i], "(\\p{P})", "");
                                    configuration.BlacklistWords.Add(listOfWords[i]);
                                }
                            }
                        }
                        catch (FileNotFoundException ex)
                        {
                            throw new ArgumentException("Input file not found.", ex);
                        }
                        catch (PathTooLongException ex)
                        {
                            throw new ArgumentException("Specified path too long.", ex);
                        }
                        catch (FileLoadException ex)
                        {
                            throw new ArgumentException("File could not be loaded.", ex);
                        }
                        catch (EndOfStreamException ex)
                        {
                            throw new ArgumentException("Reading attempted past end of stream.", ex);
                        }
                        catch (DirectoryNotFoundException ex)
                        {
                            throw new ArgumentException("Part of a file or directory cannot be found", ex);
                        }
                    }
                }
            }
            else if (arguments.Length > 4)
            {
                throw new ArgumentException("Too many parameters provided");
            }
            return configuration;
        }
    }
}

namespace MindLink.Recruitment.MyChat.UI.Console.Controllers
{
    using Application.Commands;
    using Application.Data;
    using Application.Handlers;
    using Application.Results;
    using Serialization;
    using System;
    using System.IO;
    using System.Security;
    using System.Text;

    /// <summary>
    /// Conversation controller.
    /// </summary>
    public sealed class ConversationController: IConversationController
    {
        private readonly ICommandHandler<ExportConversationCommand, ExportConversationResult> _exportConversationHandler;
        private readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a <see cref="ConversationController"/>
        /// </summary>
        /// <param name="conversationHandler">Handler for <see cref="ExportConversationCommand"/></param>
        /// <param name="serializer">Value serializer.</param>
        public ConversationController(ICommandHandler<ExportConversationCommand, ExportConversationResult> conversationHandler, ISerializer serializer)
        {
            _exportConversationHandler = conversationHandler;
            _serializer = serializer;
        }

        /// <summary>
        /// Exports a conversation
        /// </summary>
        /// <param name="options">The options of the export operation.</param>
        public void Export(Options.Options options)
        {
            ExportConversationCommand command = CreateExportConversationCommand(options);
            ExportConversationResult result = _exportConversationHandler.Handle(command);

            if (!result.IsSuccess)
            {
                foreach (var error in result.Errors)
                    Console.WriteLine(error);
            }
            else
            {
                WriteToFile(options.OutputFile, result.Conversation);
                Console.WriteLine(string.Format(Resources.ConversationExportedToFileFormat, options.InputFile, options.OutputFile));
            }
        }

        /// <summary>
        /// Creates an <see cref="ExportConversationCommand"/> from the provided <see cref="Options.Options"/>.
        /// </summary>
        /// <param name="options">The options to use.</param>
        /// <returns>The generated command.</returns>
        private ExportConversationCommand CreateExportConversationCommand(Options.Options options)
        {
            byte[] input = ReadFile(options.InputFile);

            return new ExportConversationCommand
            {
                Input = input,
                StreamEncoding = Encoding.ASCII,
                UserIdFilter = options.UserFilter,
                ContentKeywordFilter = options.ContentKeywordFilter,
                KeywordsToCensor = options.KeywordsToCensor,
                CensorCreditCardNumbers = options.CensorSensitiveInformation,
                CensorTelephoneNumbers = options.CensorSensitiveInformation,
                ObfuscateUserId = options.ObfuscateUser,
                GenerateMostActiveUsersReport = options.GenerateMostActiveUsersReport,
            };
        }

        /// <summary>
        /// Reads a file from disk into a byte array.
        /// </summary>
        /// <param name="inputFile">The path of the input file.</param>
        /// <returns>The byte array with the contents of the file.</returns>
        private byte[] ReadFile(string inputFile)
        {
            try
            {
                using (var fileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                {
                    MemoryStream memStream = new MemoryStream();
                    fileStream.CopyTo(memStream);
                    return memStream.ToArray();
                }
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException("The file was not found.");
            }
            catch (IOException)
            {
                throw new Exception("Something went wrong in the IO.");
            }
        }

        /// <summary>
        /// Writes a byte array to a file on disk.
        /// </summary>
        /// <param name="outputFile">The name of the output file to write the contents into.</param>
        /// <param name="content">The contents to write.</param>
        private void WriteToFile(string outputFile, ConversationDTO conversation)
        {
            try
            {
                using (var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.ReadWrite))
                {
                    _serializer.Serialize(conversation, outputStream);
                }
            }
            catch (SecurityException)
            {
                throw new ArgumentException("No permission to file.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("Path invalid.");
            }
        }
    }
}

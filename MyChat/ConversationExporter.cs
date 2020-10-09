namespace MyChat
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Security;
	using System.Text;
	using Microsoft.Extensions.Configuration;
	using MindLink.Recruitment.MyChat;
	using Newtonsoft.Json;

	/// <summary>
	/// Represents a conversation exporter that can read a conversation and write it out in JSON.
	/// </summary>
	public sealed class ConversationExporter
	{
		/// <summary>
		/// The application entry point.
		/// </summary>
		/// <param name="args">
		/// The command line arguments.
		/// </param>

		public ConversationExporter() { }
		static void Main(string[] args)
		{
			// We use Microsoft.Extensions.Configuration.CommandLine and Configuration.Binder to read command line arguments.
			var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
			var exporterConfiguration = configuration.Get<ConversationExporterConfiguration>();
			var conversationExporter = new ConversationExporter();
			var option = "";

			Console.WriteLine("Please enter the option you would like to execute: \n\n" +
						 "1. To export entire conversation to a JSON file, type in --export \n" +
						 "2. To filter messages by a user and export to a JSON file, type in --filterByUser <user> \n" +
						 "3. To filter messages by a keyword and export to a JSON file, type in --filterByKeyword <keyword> \n" +
						 "4. To hide specific words from the messages and export to a JSON file, type in --blacklist <word1>,<word2> \n" +
						 "5. To create a report of the number of messages each user sent and export to a JSON file, type in --report");

			option = Console.ReadLine();

			if (option == "--export")
			{
				conversationExporter.ExportConversation(exporterConfiguration.InputFilePath, exporterConfiguration.OutputFilePath);

			}
			else if (option.Contains("--filterByUser "))
			{
				var split = option.Split(' ');
				var user = split[1];
				conversationExporter.FilterByUser(user, exporterConfiguration.InputFilePath, exporterConfiguration.OutputFilePath);
			}
			else if (option.Contains("--filterByKeyword "))
			{
				var split = option.Split(' ');
				var keyword = split[1];
				conversationExporter.FilterByKeyword(keyword, exporterConfiguration.InputFilePath, exporterConfiguration.OutputFilePath);
			}
			else if (option.Contains("--blacklist "))
			{
				var commandSplit = option.Split(' ');
				var valueSplit = commandSplit[1].Split(',');

				List<string> wordsToExcludeFromMesssages = new List<string>();

				for (int i = 0; i < valueSplit.Length; i++)
				{
					wordsToExcludeFromMesssages.Add(valueSplit[i]);
				}

				conversationExporter.BlackList(wordsToExcludeFromMesssages, exporterConfiguration.InputFilePath, exporterConfiguration.OutputFilePath);
			}
			else if (option.Contains("--report"))
			{
				conversationExporter.Report(exporterConfiguration.InputFilePath, exporterConfiguration.OutputFilePath);

			}
			else
			{
				Console.WriteLine("Invalid option entry! Please choose an option as described above");
			}
		}

		/// <summary>
		/// Exports the conversation at <paramref name="inputFilePath"/> as JSON to <paramref name="outputFilePath"/>.
		/// </summary>
		/// <param name="inputFilePath">
		/// The input file path.
		/// </param>
		/// <param name="outputFilePath">
		/// The output file path.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Thrown when a path is invalid.
		/// </exception>
		/// <exception cref="Exception">
		/// Thrown when something bad happens.
		/// </exception>
		public void ExportConversation(string inputFilePath, string outputFilePath)
		{
			Conversation conversation = this.ReadConversation(inputFilePath);

			this.WriteConversation(conversation, outputFilePath);
			Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);

		}

		public void FilterByUser(string user, string inputFilePath, string outputFilePath)
		{
			Conversation conversation = this.ReadConversation(inputFilePath);

			var filteredByUserMessages = new List<Message>();
			filteredByUserMessages = conversation.messages.ToList();
			filteredByUserMessages = filteredByUserMessages.Where(message => message.senderId.ToLower() == user.ToLower()).ToList();
			conversation.messages = filteredByUserMessages;

			if (!filteredByUserMessages.Any())
			{
				Console.WriteLine("No messages have been sent by this user");
			}
			else
			{
				this.WriteConversation(conversation, outputFilePath);
				Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
			}
		}

		public void Report(string inputFilePath, string outputFilePath)
		{
			Conversation conversation = this.ReadConversation(inputFilePath);

			List<User> activity = new List<User>();
			List<Message> conversationMessages = new List<Message>();
			conversationMessages = conversation.messages.ToList();

			for (int i = 0; i < conversationMessages.Count(); i++)
			{
				int index = activity.FindIndex(user => user.sender == conversationMessages[i].senderId);
				if (index >= 0)
				{
					activity[index].Count++;
				}
				else
				{
					User user = new User(conversationMessages[i].senderId);
					user.Count++;
					activity.Add(user);
				}
			}

			activity.Sort((x, y) => y.Count.CompareTo(x.Count));

			this.WriteConversation(new Conversation(conversation.name, conversation.messages, activity), outputFilePath);
			Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);

		}

		public void FilterByKeyword(string keyword, string inputFilePath, string outputFilePath)
		{
			Conversation conversation = this.ReadConversation(inputFilePath);

			var filteredByKeywordMessages = new List<Message>();
			filteredByKeywordMessages = conversation.messages.ToList();
			filteredByKeywordMessages = filteredByKeywordMessages.Where(message => message.content.ToLower().Contains(keyword.ToLower())).ToList();
			conversation.messages = filteredByKeywordMessages;

			if (!filteredByKeywordMessages.Any())
			{
				Console.WriteLine("No messages include the keyword: {0}", keyword);
			}
			else
			{
				this.WriteConversation(conversation, outputFilePath);
				Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);
			}

		}

		public void BlackList(List<string> wordsToExcludeFromMesssages, string inputFilePath, string outputFilePath)
		{

			Conversation conversation = this.ReadConversation(inputFilePath);

			var messages = new List<Message>();
			messages = conversation.messages.ToList();

			for (int i = 0; i < messages.Count; i++)
			{
				for (int j = 0; j < wordsToExcludeFromMesssages.Count(); j++)
				{
					if (messages[i].content.Contains(wordsToExcludeFromMesssages[j]))
					{
						messages[i].content = messages[i].content.Replace(wordsToExcludeFromMesssages[j], "*redacted*");
					}
				}
			}

			this.WriteConversation(new Conversation(conversation.name, messages), outputFilePath);
			Console.WriteLine("Conversation exported from '{0}' to '{1}'", inputFilePath, outputFilePath);

		}

		/// <summary>
		/// Helper method to read the conversation from <paramref name="inputFilePath"/>.
		/// </summary>
		/// <param name="inputFilePath">
		/// The input file path.
		/// </param>
		/// <returns>
		/// A <see cref="Conversation"/> model representing the conversation.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Thrown when the input file could not be found.
		/// </exception>
		/// <exception cref="Exception">
		/// Thrown when something else went wrong.
		/// </exception>
		public Conversation ReadConversation(string inputFilePath)
		{
			try
			{
				var reader = new StreamReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read),
					Encoding.ASCII);

				string conversationName = reader.ReadLine();
				var messages = new List<Message>();

				string line;

				while ((line = reader.ReadLine()) != null)
				{
					var split = line.Split(' ');

					string messageContent = "";

					for (int i = 2; i < split.Length; i++)
					{
						if (i == split.Length - 1)
						{
							messageContent += split[i];
						}
						else
						{
							messageContent += split[i] + " ";
						}
					}

					messages.Add(new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(split[0])), split[1], messageContent));
				}

				return new Conversation(conversationName, messages);
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
		/// Helper method to write the <paramref name="conversation"/> as JSON to <paramref name="outputFilePath"/>.
		/// </summary>
		/// <param name="conversation">
		/// The conversation.
		/// </param>
		/// <param name="outputFilePath">
		/// The output file path.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Thrown when there is a problem with the <paramref name="outputFilePath"/>.
		/// </exception>
		/// <exception cref="Exception">
		/// Thrown when something else bad happens.
		/// </exception>
		public void WriteConversation(Conversation conversation, string outputFilePath)
		{
			try
			{
				var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

				var serialized = JsonConvert.SerializeObject(conversation, Formatting.Indented);

				writer.Write(serialized);

				writer.Flush();

				writer.Close();
			}
			catch (SecurityException)
			{
				throw new ArgumentException("No permission to file.");
			}
			catch (DirectoryNotFoundException)
			{
				throw new ArgumentException("Path invalid.");
			}
			catch (IOException)
			{
				throw new Exception("Something went wrong in the IO.");
			}
		}
	}
}

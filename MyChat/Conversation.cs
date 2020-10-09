namespace MindLink.Recruitment.MyChat
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Represents the model of a conversation.
	/// </summary>
	public sealed class Conversation
	{
		/// <summary>
		/// The name of the conversation.
		/// </summary>
		public string name;

		/// <summary>
		/// The messages in the conversation.
		/// </summary>
		public IEnumerable<Message> messages;

		/// <summary>
		/// The activity in the conversation.
		/// </summary>
		public IEnumerable<User> activity;

		/// <summary>
		/// Initializes a new instance of the <see cref="Conversation"/> class.
		/// </summary>
		/// <param name="name">
		/// The name of the conversation.
		/// </param>
		/// <param name="messages">
		/// The messages in the conversation.
		/// </param>
		/// 

		public Conversation() { }
		public Conversation(string name, IEnumerable<Message> messages)
		{
			this.name = name;
			this.messages = messages;
		}

		// <summary>
		/// Initializes a new instance of the <see cref="Conversation"/> class.
		/// </summary>
		/// <param name="name">
		/// The name of the conversation.
		/// </param>
		/// <param name="messages">
		/// The messages in the conversation.
		/// </param>
		/// <param name="activity">
		/// The number of users and their respective number of sent messages within a conversation.
		/// </param>

		public Conversation(string name, IEnumerable<Message> messages, IEnumerable<User> activity)
		{
			this.name = name;
			this.messages = messages;
			this.activity = activity;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("Conversation name: " + name);

			if (messages.Any())
			{
				foreach (Message message in messages)
				{
					sb.AppendLine(message.ToString());
				}
			}
			return sb.ToString();
		}
	}
}

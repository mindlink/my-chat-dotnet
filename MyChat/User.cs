using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Represents an activity of a chat.
/// </summary>

namespace MindLink.Recruitment.MyChat
{
	public class User
	{
		/// <summary>
		/// The message count per user.
		/// </summary>

		public int Count { get; set; }

		/// <summary>
		/// The message sender.
		/// </summary>
		public string sender;

		/// <summary>
		/// Initializes a new instance of the <see cref="User"/> class.
		/// </summary>
		/// <param name="sender">
		/// The ID of the sender.
		/// </param>
		/// <param name="Count">
		/// The message content.
		/// </param>
		public User(string sender, int count)
		{
			this.sender = sender;
			this.Count = count;
		}

		public User(string senderId)
		{
			this.sender = senderId;
		}

		public override string ToString()
		{
			return sender + " " + Count;
		}
	}
}

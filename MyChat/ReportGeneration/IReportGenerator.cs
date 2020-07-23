namespace MindLink.Recruitment.MyChat.ReportGeneration
{
    using System.Collections.Generic;

    using MindLink.Recruitment.MyChat.ConversationData;

    /// <summary>
    /// Generates user activity reports for <see cref="Conversation"/> objects.
    /// </summary>
    public interface IReportGenerator
    {
        IDictionary<string, int> Generate(Conversation conversation);
    }
}
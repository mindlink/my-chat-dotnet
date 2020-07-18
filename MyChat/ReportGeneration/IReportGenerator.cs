namespace MindLink.Recruitment.MyChat
{
    using System.Collections.Generic;

    /// <summary>
    /// Generates user activity reports for <see cref="Conversation"/> objects.
    /// </summary>
    public interface IReportGenerator
    {
        IDictionary<string, int> Generate(Conversation conversation);
    }
}
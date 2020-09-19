namespace MindLink.Recruitment.MyChat.Interfaces.ControllerInterfaces
{
    using MyChatModel.ModelData;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IReportController
    {
        /// <summary>
        /// GenerateReport from the conversation 
        /// </summary>
        /// <param name="conversation"> <see cref="Conversation"> variable used for the report </param>
        /// <returns> the conversation with the generated report </returns>
        Conversation GenerateReport(Conversation conversation);
    }
}

using MyChatModel.ModelData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Interfaces.FeatureInterfaces
{
    public interface IStrategyFilter
    {

        Conversation ApplyFilter(Conversation conversation);
    }
}

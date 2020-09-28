using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public class FilterChainMaker : IFilterChainMaker
    {
        public IFilterer CreateFilterChain(ConversationExporterConfiguration config)
        {
            List<IFilterer> chain = new List<IFilterer>();
            
            if (config.UserToFilter != null)
            {
                chain.Add(new UserNameFilterer(config.UserToFilter));
            }
            
            if (config.KeywordToFilter != null)
            {
                chain.Add(new KeywordFilterer(config.KeywordToFilter));
            }

            if (chain.Count < 1)
            {
                //No filters were specified.
                return null;
            }
            
            //Now loop through the chain and hook up i with i+1
            for (int i = 0; i < chain.Count-1; i++)
            {
                chain[i].NextFilterer = chain[i + 1];
            }
            
            return chain[0];
        }
    }

    public interface IFilterChainMaker
    {
        IFilterer CreateFilterChain(ConversationExporterConfiguration c);
    }
}
using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat
{
    public static class AdjusterChainMaker
    {
        public static IAdjuster CreateAdjusterChain(ConversationExporterConfiguration config)
        {
            List<IAdjuster> chain = new List<IAdjuster>();
            
            if (config.BannedTerm != null)
            {
                chain.Add(new BannedTermRedactor(config.BannedTerm));
            }
            
            if (chain.Count < 1)
            {
                //No adjusters were specified.
                return null;
            }
            
            //Now loop through the chain and hook up i with i+1
            for (int i = 0; i < chain.Count-1; i++)
            {
                chain[i].NextAdjuster = chain[i + 1];
            }
            
            return chain[0];
        }    
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindLink.Recruitment.MyChat
{
     public class BannedTermRedactor : IAdjuster
     {
         public IAdjuster NextAdjuster { get; set; }
         
         public string BannedTerm { get; set; }

         public BannedTermRedactor(string bannedTerm)
         {
             BannedTerm = bannedTerm;
         }

         public IMessage Adjust(IMessage m)
         {
            //Here is where we do the actual adjustment work.
            var sanitised = SanitiseMessage(m);
            if (NextAdjuster != null)
            {
                return NextAdjuster.Adjust(sanitised);
            }

            return sanitised;
         }

         public IMessage SanitiseMessage(IMessage message)
         {
             //SanitiseMessage takes in a message and removes the bannedTerm that's stored on the class.
             //It isn't case sensitive but it is 'aware' of terminal punctuation. That means a word of "hello" and "hello!" will be "*redacted*!" in the output. 
             
             //This is our first check. Ostensibly, it's used to see if the banned word is anywhere in the message. 
             //But, because we inject this filterer into the class, it can actually search for anything. 
             //So, theoretically, if you wanted to redact a word __only__ if the message contained a reference to a keyword or user or date, well that's possible now.
             
             List<string> final = new List<string>();
             
             foreach (var word in message.content.Split(" "))
             {
                 
                 //The word and the banned term match exactly:
                 if (String.Equals(word, BannedTerm, StringComparison.CurrentCultureIgnoreCase))
                 {
                     final.Add("*redacted*");
                 }
                 
                 //Would the words match if you tried to remove terminal punctuation?
                 else if (WordsMatchAfterTerminalPunctuationRemoved(FindStartOfTerminalPunctuation_English(word),
                     word, BannedTerm))
                 {
                     //Turns out the words do match. 
                     //So, redact the word and add in the punctuation after.
                     final.Add($"*redacted*{word.Substring(FindStartOfTerminalPunctuation_English(word))}");
                 }

                 //The word and banned term don't match.
                 else
                 {
                     final.Add(word);
                 }
             }
             
             return new Message(message.timestamp, message.senderId, string.Join(" ", final));
         }

         public int FindStartOfTerminalPunctuation_English(string word)
         {
             // FinsStartOfTerminalPunctuation_English searches through a word and returns the point at which the first
             // piece of terminal punctuation is found. It will return -1 if no punctuation is found or if the first piece
             // of punctuation is followed by alphanumeric characters. 
         
             // This function exists to help with a specific case where the word you're looking for is affected by
             // terminal punctuation. For example, if the keyword is 'pie' and you find 'pie!'.
         
             // The _English suffix foregrounds that this is brittle. If we start searching through a Spanish chat log
             // and messages start with ¿, then we'll struggle.
         
             //Find out where the first piece of punctuation exists.
             var FirstPuncIndex = word.ToList().FindIndex(char.IsPunctuation);
             
             //If we didn't find any punctuation, then there's none at all, so there's nothing to be done.
             if(FirstPuncIndex == -1)
             {
                 return -1;
             }

             //Now check that only punctuation follows after the first occurence of punctuation in the word.
             bool OnlyPunctuationAfterFirst = word.Substring(FirstPuncIndex).All(char.IsPunctuation);
             
             if (!OnlyPunctuationAfterFirst)
             {
                 // We bail here because we've either not found punctuation OR we've found punctuation followed by letters 
                 return -1;
             }
         
             return FirstPuncIndex;
         }
         
         public bool WordsMatchAfterTerminalPunctuationRemoved(int index, string word, string bannedTerm)
         {
             //If there's a non-zero index, we can infer there might be terminal punctuation that exists in a given word. 
             //If that's the case and you get rid of that chunk of punctuation, do the word and the bt word match?
             return index != -1 &&
                    String.Equals(word.Substring(0, index), bannedTerm, StringComparison.CurrentCultureIgnoreCase);
         }


     }
}
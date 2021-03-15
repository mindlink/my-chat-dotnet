using System;
using System.Collections.Generic;
using System.Text;

namespace MindLink.Recruitment.MyChat.Utilities
{
    //class used to store static test utilities function, like a general purpose helper class
    public static class TextFunctions
    {
        //this function is similar to String.Replace() but covers more edge cases and checks the whole word and not a portion of the word
        public static string ReplaceWords(string sentence, string oldWord, string newWord)
        {
            string currentWord = "";
            string outSentense = "";
            string heldPunctuation = "";

            for (int i = 0; i < sentence.Length; i++)
            {
                if ((sentence[i].Equals(' ')) || i == sentence.Length - 1)
                {
                    //checks if this is an end punctuation, e.g. 'pie.'
                    if (i == sentence.Length - 1)
                    {
                        if (Char.IsPunctuation(sentence[i]))
                        {
                            heldPunctuation = heldPunctuation + sentence[i];
                        }
                        else
                            currentWord = currentWord + sentence[i];
                    }

                    if (currentWord.Equals(oldWord))
                    {
                        outSentense = outSentense + newWord + heldPunctuation + " ";
                    }
                    else
                    {
                        outSentense = outSentense + currentWord + heldPunctuation + " ";
                    }

                    currentWord = "";
                    heldPunctuation = "";
                }

                //case that handles words such as That'll
                else if (sentence[i].Equals('\''))
                {
                    currentWord = currentWord + sentence[i];
                }

                //case that handles any kind of punctuation
                else if (Char.IsPunctuation(sentence[i]))
                {
                    if (sentence[i - 1].Equals(' ') || i == 0)
                    {
                        outSentense += sentence[i];
                    }
                    else
                        heldPunctuation = heldPunctuation + sentence[i];
                }
                else
                {
                    currentWord = currentWord + sentence[i];
                }
            }

            //trims then returns the output
            return outSentense.Trim();
        }
    }
}

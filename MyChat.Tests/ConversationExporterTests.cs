using System.Collections.Generic;
using System.IO;
using System.Text;
using MyChat;
using Newtonsoft.Json;
using NUnit.Framework;


namespace MindLink.Recruitment.MyChat.Tests
{
    using System;

    [TestFixture]
    public class ConversationExporterTests
    {
        [Test]
        public void TitleOfExportedConversationIsCorrect()
        {
            var exporter = new ConversationExporter();

            var emptyConfig = new ConversationExporterConfiguration();

            List<Func<Message, bool>> emptyRules = new List<Func<Message, bool>>();

            var reader = exporter.GetStreamReader("chat.txt", FileMode.Open,
                FileAccess.Read, Encoding.ASCII);

            var writer = exporter.GetStreamWriter("output.json", FileMode.Create, FileAccess.ReadWrite);

            exporter.WriteConversation(writer, exporter.ExtractConversation(reader, emptyRules, emptyConfig));

            var serializedConversation = new StreamReader(new FileStream("chat.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.Name, Is.EqualTo("My Conversation"));
        }

        [Test]
        public void CorrectArgsReturnsCorrectStreamReader()
        {
            var cp = new ConversationExporter();
            var got = cp.GetStreamReader("chat.txt", FileMode.Create, FileAccess.ReadWrite, Encoding.ASCII);
            Assert.That(got, Is.TypeOf<StreamReader>());
        }

        // //
        // // // [Test]
        // // // public void NonExistentDirectoryToStreamReaderThrowsError()
        // // // {
        // // //TODO: fill test.
        // // // }
        // //
        [Test]
        public void CorrectArgsReturnsCorrectStreamWriter()
        {
            var cp = new ConversationExporter();
            var got = cp.GetStreamWriter("output.something", FileMode.Create, FileAccess.ReadWrite);
            Assert.That(got, Is.TypeOf<StreamWriter>());
        }

        [Test]
        public void ArrayToMessageTakesArrayAndReturnsMessageObject()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};
            var got = cp.ArrayToMessage(line);
            Assert.That(got, Is.TypeOf<Message>());
        }

        [Test]
        public void SenderIDCorrectAfterConversionIntoMessage()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};
            var got = cp.ArrayToMessage(line).senderId;
            var want = "david";
            Assert.That(got, Is.EqualTo(want));
        }

        [Test]
        public void MessageContentCorrectAfterConversion()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};
            var got = cp.ArrayToMessage(line).content;
            var want = "a message";
            Assert.That(got, Is.EqualTo(want));
        }

        [Test]
        public void TimestampCorrectWhenCreatingNewMessage()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1448470901", "david", "a message"};
            var got = cp.ArrayToMessage(line).timestamp;
            DateTimeOffset want = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901"));
            Assert.That(got, Is.EqualTo(want));
        }

        [Test]
        public void StringToUnixTimeStampCorrrectlyFormatsTimeStamp()
        {
            ConversationExporter cp = new ConversationExporter();
            DateTimeOffset got = cp.StringToUnixTimeStamp("1448470901");
            DateTimeOffset want = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901"));
            Assert.That(got, Is.EqualTo(want));
        }

        [Test]
        public void NameInSenderReturnsTrue()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.UsernameFound("david")(message), Is.EqualTo(true));
        }

        [Test]
        public void ShorterNameContainedWithinLongerNameIsNotFound()
        {
            // If someone searches for "davide" and we find the name is "david"
            // that should fail. 
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "davide", "a message"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.UsernameFound("david")(message), Is.EqualTo(false));
        }

        [Test]
        public void NameNotInSenderReturnsFalse()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "a message"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.UsernameFound("notInSender")(message), Is.EqualTo(false));
        }

        [Test]
        public void KeywordInMessageReturnsTrue()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "this is a message"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.KeywordInMessage("message")(message), Is.EqualTo(true));
        }

        [Test]
        public void KeywordNotInMessageReturnsFalse()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "this is a message"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.KeywordInMessage("nonexistent")(message), Is.EqualTo(false));
        }

        [Test]
        public void ShortKeywordNotFoundInLongerWordReturns()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "shorter word is found within"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.KeywordInMessage("with")(message), Is.EqualTo(false));
        }

        [Test]
        public void CapitalKeywordFindsLowercaseKeyword()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "Shorter"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.KeywordInMessage("shorter")(message), Is.EqualTo(true));
        }

        [Test]
        public void UpperConfigKeywordFindsLowercaseWordInMessage()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "lowercase in message but upper in config"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.KeywordInMessage("Upper")(message), Is.EqualTo(true));
        }

        [Test]
        public void KeywordWithinAnotherWordNotFound()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "hereisalongword found within"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.KeywordInMessage("hereisalongwordplusextra")(message), Is.EqualTo(false));
        }

        [Test]
        public void WordWithPunctuationInMessageStillFoundWithMatchingKeyword()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "hi! is a message"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.KeywordInMessage("hi")(message), Is.EqualTo(true));
        }

        [Test]
        public void CasingDoesNotMatter()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "hi is a message"};

            var message = cp.ArrayToMessage(line);


            Assert.That(cp.KeywordInMessage("HI")(message), Is.EqualTo(true));
        }

        [Test]
        public void KeywordWithPunctuationAlsoFindsWordWithPunctuation()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "hi! is a message"};

            var message = cp.ArrayToMessage(line);


            Assert.That(cp.KeywordInMessage("hi!")(message), Is.EqualTo(true));
        }

        [Test]
        public void CasingAndPunctuationStillFound()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "Hi! a message"};

            var message = cp.ArrayToMessage(line);


            Assert.That(cp.KeywordInMessage("hi")(message), Is.EqualTo(true));
        }

        [Test]
        public void MessageContainingPartOfKeywordIsFalse()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "thisisalongstring"};

            var message = cp.ArrayToMessage(line);

            Assert.That(cp.KeywordInMessage("string")(message), Is.EqualTo(false));
        }

        [Test]
        public void IwithApostropheIsNotFoundWhenKeywordisI()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "I'm like pie."};
            var message = cp.ArrayToMessage(line);
            Assert.That(cp.KeywordInMessage("I")(message), Is.EqualTo(false));
        }

        [Test]
        public void KeywordWithApostopheFindsWordWithApostrophe()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "don't like pie."};
            var message = cp.ArrayToMessage(line);
            Assert.That(cp.KeywordInMessage("don't")(message), Is.EqualTo(true));
        }

        [Test]
        public void BannedWordRemovedFromMessage()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "banned word is found"};
            var message = cp.ArrayToMessage(line);
            var bt = "word";
            Assert.That(cp.SanitiseMessage(message, "word").content, Is.EqualTo("banned *redacted* is found"));
        }

        [Test]
        public void NonExistentBannedTermNotRemovedFromMessage()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "banned term is not found"};
            var message = cp.ArrayToMessage(line);
            var bt = "word";
            Assert.That(cp.SanitiseMessage(message, "other").content, Is.EqualTo("banned term is not found"));
        }

        [Test]
        public void BannedWordContainedWithinALongerWordIsNotRedacted()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "banned word contained within is not changed"};
            var message = cp.ArrayToMessage(line);
            var bt = "with";
            Assert.That(cp.SanitiseMessage(message, "with").content,
                Is.EqualTo("banned word contained within is not changed"));
        }

        [Test]
        public void CasingOfWordsNotImportantForBannedTermTerms()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "casing of bannedterm word not significant"};
            var message = cp.ArrayToMessage(line);
            Assert.That(cp.SanitiseMessage(message, "Word").content,
                Is.EqualTo("casing of bannedterm *redacted* not significant"));
        }

        [Test]
        public void FullstopAtEndOfBannedTermIsStillRedacted()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "I like pie."};
            var message = cp.ArrayToMessage(line);
            Assert.That(cp.SanitiseMessage(message, "pie").content, Is.EqualTo("I like *redacted*."));
        }


        [Test]
        public void ExclamationMarkAtEndOfWordRemoved()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "hi! should be removed."};
            var message = cp.ArrayToMessage(line);
            Assert.That(cp.SanitiseMessage(message, "hi").content, Is.EqualTo("*redacted*! should be removed."));
        }

        [Test]
        public void ApostropheInWordRemovedIfBanndTermIsSame()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "I'm should be removed."};
            var message = cp.ArrayToMessage(line);
            Assert.That(cp.SanitiseMessage(message, "I'm").content, Is.EqualTo("*redacted* should be removed."));
        }
        
        [Test]
        public void ApostopheInWordButKeywordSomeWhereElse()
        {
            //Essentially making sure we don't have a banned term like 'I' get redacted from 'I'm' in a message
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "I'm okay where I am."};
            var message = cp.ArrayToMessage(line);
            Assert.That(cp.SanitiseMessage(message, "I").content, Is.EqualTo("I'm okay where *redacted* am."));
        }


        [Test]
        public void EllipsisAtEndOfRedactedWordRetained()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "I want pie now..."};
            var message = cp.ArrayToMessage(line);
            Assert.That(cp.SanitiseMessage(message, "now").content, Is.EqualTo("I want pie *redacted*..."));
        }
        
        [Test]
        public void QuestionMarksMidSentenceAreRetained()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "You're kidding right??? Everyone loves pie."};
            var message = cp.ArrayToMessage(line);

            Assert.That(cp.SanitiseMessage(message, "right").content, Is.EqualTo("You're kidding *redacted*??? Everyone loves pie."));
        }

        [Test]
        public void CommaAfterAWordIsRetained()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "I, to be honest, really want pie"};
            var message = cp.ArrayToMessage(line);
            Assert.That(cp.SanitiseMessage(message, "honest").content,
                Is.EqualTo("I, to be *redacted*, really want pie"));
        }
        
        [Test]
        public void ComboOfCapitalAndPunctuationStillRedacted()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "Hi! there"};
            var message = cp.ArrayToMessage(line);
            Assert.That(cp.SanitiseMessage(message, "hi").content,
                Is.EqualTo("*redacted*! there"));
        }

        [Test]
        public void RemovePunctuationFromWords()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "hi!"};
            var message = cp.ArrayToMessage(line);

            Assert.That(cp.StripPunctuation(message.content), Is.EqualTo("hi"));
        }

        [Test]
        public void NoPunctuationInMessageMeansNoAdjustment()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "hello"};
            var message = cp.ArrayToMessage(line);

            Assert.That(cp.StripPunctuation(message.content), Is.EqualTo("hello"));
        }
        
        [Test]
        public void FindsEllipsis()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "pie..."};
            var message = cp.ArrayToMessage(line);

            Assert.That(cp.FindStartOfTerminalPunctuation_English(message.content), Is.EqualTo(3));
        }
        
        [Test]
        public void FindsCommaAfterLetters()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "nice,"};
            var message = cp.ArrayToMessage(line);

            Assert.That(cp.FindStartOfTerminalPunctuation_English(message.content), Is.EqualTo(4));
        }

        [Test]
        public void BailsIfFindsApostophe()
        {
            ConversationExporter cp = new ConversationExporter();
            string[] line = {"1234", "david", "I'm"};
            var message = cp.ArrayToMessage(line);

            Assert.That(cp.FindStartOfTerminalPunctuation_English(message.content), Is.EqualTo(-1));
        }

        [Test]
        public void RemovingFullStopsMeansWordsMatch()
        {
            ConversationExporter cp = new ConversationExporter();

            var got = cp.WordsMatchAfterTerminalPunctuationRemoved(2, "hi!", "hi");
            Assert.That(got, Is.EqualTo(true));
        }
        
        [Test]
        public void WordsDontMatchEvenWithRemovalOfTerminalPuncs()
        {
            ConversationExporter cp = new ConversationExporter();

            var got = cp.WordsMatchAfterTerminalPunctuationRemoved(5, "hello!!!", "howdy");
            Assert.That(got, Is.EqualTo(false));
        }
    }
}
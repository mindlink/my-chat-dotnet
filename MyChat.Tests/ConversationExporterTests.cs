// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text;
// using Microsoft.VisualStudio.TestPlatform.Utilities;
// using MyChat;
// using Newtonsoft.Json;
// using NUnit.Framework;
//
//
// namespace MindLink.Recruitment.MyChat.Tests
// {
//      using System;
//
//      [TestFixture]
//      public class ConversationExporterTests
//      {
//          [Test]
//          public void TitleOfExportedConversationIsCorrect()
//          {
//              var exporter = new ConversationExporter();
//
//              var reader = TextReaderFactory.GetStreamReader("chat.txt", FileMode.Open,
//                  FileAccess.Read, Encoding.ASCII);
//
//              var writer = TextWriterFactory.GetStreamWriter("output.json", FileMode.Create, FileAccess.ReadWrite);
//
//              exporter.WriteConversation(writer, exporter.ExtractConversation(reader, null, null));
//
//              var serializedConversation = new StreamReader(new FileStream("output.json", FileMode.Open)).ReadToEnd();
//              Console.WriteLine(serializedConversation);
//              var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
//
//              Assert.That(savedConversation.Name, Is.EqualTo("My Conversation"));
//          }
//
//          [Test]
//          public void first_message_item_is_from_david()
//          {
//              var cp = new ConversationExporter();
//              IFilterer dg = new UserNameFilterer("david");
//              
//              TextReader reader = TextReaderFactory.GetStreamReader("chat.txt", FileMode.Open,
//                  FileAccess.Read, Encoding.ASCII);
//
//              TextWriter writer = TextWriterFactory.GetStreamWriter("output.json", FileMode.Create, FileAccess.ReadWrite);
//
//              Conversation conversation = cp.ExtractConversation(reader, dg, null);
//              
//              writer = TextWriterFactory.GetStreamWriter("output.json", FileMode.Create,
//                  FileAccess.ReadWrite);
//              
//              cp.WriteConversation(writer, conversation);
//              
//              var serializedConversation = new StreamReader(new FileStream("output.json", FileMode.Open)).ReadToEnd();
//              
//              var convo = JsonConvert.DeserializeObject<Conversation>(serializedConversation);
//              
//              string cname = convo.Name;
//              Console.WriteLine(cname);
//
//          } 
//      }
// }
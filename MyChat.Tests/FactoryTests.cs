using System.IO;
using System.Text;
using MyChat;
using NUnit.Framework;
namespace MindLink.Recruitment.MyChat.Tests
{
    public class FactoryTests
    {
     [Test]
     public void CorrectArgsReturnsCorrectStreamReader()
     {
         var config = new ConversationExporterConfiguration();
         config.inputFilePath = "niceFile";
         var r = new TextReaderFactory();
         
         var reader = r.GetStreamReader(config);
         
         Assert.That(reader, Is.TypeOf<StreamReader>());
     }

     [Test]
     public void CorrectArgsReturnsCorrectStreamWriter()
     {
         var config = new ConversationExporterConfiguration();
         config.outputFilePath = "output.something";
         var tw = new TextWriterFactory(); 
         
         var writer = tw.GetStreamWriter(config);
         
         Assert.That(writer, Is.TypeOf<StreamWriter>());
     }
    }
}
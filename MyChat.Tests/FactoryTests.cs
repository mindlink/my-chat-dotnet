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
         var reader =
             TextReaderFactory.GetStreamReader("chat.txt", FileMode.Create, FileAccess.ReadWrite, Encoding.ASCII);
         Assert.That(reader, Is.TypeOf<StreamReader>());
         
     }

     [Test]
     public void CorrectArgsReturnsCorrectStreamWriter()
     {
         var writer = TextWriterFactory.GetStreamWriter("output.something", FileMode.Create, FileAccess.ReadWrite);
         Assert.That(writer, Is.TypeOf<StreamWriter>());
     }
    }
}
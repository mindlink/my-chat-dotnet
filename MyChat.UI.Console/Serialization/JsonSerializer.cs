namespace MindLink.Recruitment.MyChat.UI.Console.Serialization
{
    using Newtonsoft.Json;
    using System;
    using System.IO;

    public sealed class JsonSerializer : ISerializer
    {
        public byte[] Serialize(object value)
        {
            MemoryStream memStream = new MemoryStream();
            Serialize(value, memStream);
            return memStream.ToArray();
        }

        public void Serialize(object value, Stream outputStream)
        {
            if (outputStream == null)
                throw new ArgumentNullException($"The value of '{nameof(outputStream)}' cannot be null.");

            string serialized = JsonConvert.SerializeObject(value, Formatting.Indented);

            try
            {
                var writer = new StreamWriter(outputStream);
                writer.Write(serialized);
                writer.Flush();
                writer.Close();
            }
            catch (IOException)
            {
                throw new Exception("Something went wrong with the serialization of the conversation.");
            }
        }
    }
}

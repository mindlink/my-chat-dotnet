namespace MindLink.Recruitment.MyChat.UI.Console.Serialization
{
    using System.IO;

    /// <summary>
    /// Interface for a serializer.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes a value and returns the result as a byte array.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns></returns>
        /// <exception cref="SerializationErrorException"
        byte[] Serialize(object value);

        /// <summary>
        /// Serializes a value and writes the result into an output stream.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="outputStream">The stream to write the content of the serialized value.</param>
        void Serialize(object value, Stream outputStream);
    }
}

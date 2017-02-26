using MindLink.Recruitment.MyChat.Elements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.FilesProcessing
{
   public class FileExporter
    {
        /// <summary>
        /// Helper method to write the <paramref name="conversation"/> as JSON to <paramref name="outputFilePath"/>.
        /// </summary>
        /// <param name="conversation">
        /// The conversation.
        /// </param>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is a problem with the <paramref name="outputFilePath"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when something else bad happens.
        /// </exception>
        public void WriteConversationToJson(JObject outputJsonObject, string outputFilePath)
        {
            //if outputFilePath is empty we throw an exception
            if (String.IsNullOrWhiteSpace(outputFilePath))
            {
                throw new ArgumentNullException("outputFilePath", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, "outputFilePath can not be empty when exporting a conversation to a file"));
            }

            //if outputJsonObject is empty we throw an exception
            if (outputJsonObject == null)
            {
                throw new ArgumentNullException("outputFilePath", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, "outputJsonObject can not be empty when exporting a conversation to a file"));
            }

            try
            {

                var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite));

                writer.Write(outputJsonObject);

                writer.Flush();

                writer.Close();
            }
            catch (SecurityException)
            {
                throw new ArgumentException("outputFilePath", String.Format("Exception in {0}, Error message : {1}",
                         this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, "No permission to acess the output file : " + outputFilePath ));
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("outputFilePath", String.Format("Exception in {0}, Error message : {1}",
                          this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, "The output file : " + outputFilePath + " was not found."));
            }
            catch (IOException ex)
            {
                throw new IOException(String.Format("Exception in {0}, Error message : {1}",
                       this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, "Something went wrong while trying  to access the file " + outputFilePath + " Exception message : " + ex.Message));
            }
        }
}
}

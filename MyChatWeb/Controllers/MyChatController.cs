namespace MyChatWeb.Controllers
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;
    using MyChat;
    using System;
    using System.Security;
    using System.Web.Http;

    public class MyChatController : ApiController
    {
        public HttpResponseMessage Post()
        {
            string path = null;

            // Check if file was selected for upload.
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                var file = HttpContext.Current.Request.Files[0];

                // Check if the file has content.
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        CreateRequiredDirectories();

                        var fileName = Path.GetFileName(file.FileName);

                        // set the upload full path to the file.
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Chats/"), fileName);

                        // save the file on the server.
                        file.SaveAs(path);

                        // Instanciate ConversationExporter object.
                        ConversationExporter exporter = new ConversationExporter();

                        // create a unique filename for conversation output.
                        string generatedFilename = Path.Combine(HttpContext.Current.Server.MapPath("~/Chats/"), Guid.NewGuid() + ".json");

                        // Set request parameters.
                        bool encryptUsernames = HttpContext.Current.Request.Params["EncryptUsernames"] != null;
                        bool hideNumbers = HttpContext.Current.Request.Params["HideNumbers"] != null;
                        string username = HttpContext.Current.Request.Params["Username"];
                        string keyword = HttpContext.Current.Request.Params["Keyword"];
                        string blacklist = HttpContext.Current.Request.Params["Blacklist"];

                        // export the conversation to JSON.
                        exporter.ExportConversation(path, generatedFilename, username, keyword, blacklist, encryptUsernames, hideNumbers);

                        // create the response with the JSON file as its content including the header for JSON media type.
                        var response = Request.CreateResponse(HttpStatusCode.OK, File.ReadAllText(generatedFilename), MediaTypeHeaderValue.Parse("application/json"));

                        return response;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Path invalid.");
                    }
                    catch (FileNotFoundException)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "The file was not found.");
                    }
                    catch (IOException)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something went wrong in the IO.");
                    }
                    catch (SecurityException)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "No permission to file.");
                    }
                    catch (ArgumentException)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Some arguments were not valid.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Text file uploaded is empty.");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Choose a file to upload.");
            }
        }

        /// <summary>
        /// Creates necessary directories on server if they don't exist.
        /// </summary>
        private static void CreateRequiredDirectories()
        {
            // Check if upload directory exists.
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Chats/")))
            {
                // create upload directory if it does not exist.
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Chats/"));
            }

            // Check if export directory exists.
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Export/")))
            {
                // create export directory if it does not exist.
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Export/"));
            }
        }

    }
}

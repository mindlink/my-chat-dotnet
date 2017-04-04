using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using MindLink.MyChat.Domain;

namespace MindLink.MyChat.WebService.Controllers
{
    [RoutePrefix("conversation")]
    public class ConversationController : ApiController
    {
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Post()
        {
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                return this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await this.Request.Content.ReadAsMultipartAsync();

            if (provider.Contents.Count < 1)
            {
                return this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var inputStream = await provider.Contents[0].ReadAsStreamAsync();
            var response = this.Request.CreateResponse(HttpStatusCode.Created);

            response.Content = new PushStreamContent((stream, content, arg3) =>
            {
                var config = new ConversationExporterConfiguration(inputStream, stream);
                var exporter = new ConversationExporter(config);
                exporter.ExportConversation();
            }, new MediaTypeHeaderValue("application/json"));

            return response;
        }
    }
}
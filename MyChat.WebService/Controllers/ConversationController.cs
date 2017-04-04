using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using MindLink.MyChat.Domain;
using MindLink.MyChat.Domain.Filters;
using MindLink.MyChat.Domain.Transformers;

namespace MindLink.MyChat.WebService.Controllers
{
    [RoutePrefix("conversation")]
    public class ConversationController : ApiController
    {
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Post(bool sensitive = false, bool obfuscate = false, [FromUri]string[] blacklist = null, string filter = null, string user = null)
        {
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                return this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            //var provider = new MultipartFormDataStreamProvider()
            var provider = await this.Request.Content.ReadAsMultipartAsync();

            if (provider.Contents.Count < 1)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var inputStream = await provider.Contents[0].ReadAsStreamAsync();
            var response = this.Request.CreateResponse(HttpStatusCode.Created);

            response.Content = new PushStreamContent((stream, content, transportContext) =>
            {
                var configuration = new ConversationExporterConfiguration(inputStream, stream);

                if (!string.IsNullOrEmpty(filter))
                {
                    configuration.AddFilter(new KeywordFilter(filter));
                }

                if (!string.IsNullOrEmpty(user))
                {
                    configuration.AddFilter(new UserFilter(user));
                }

                if (blacklist != null)
                {
                    configuration.AddTransformer(new BlacklistTransformer(blacklist));
                }

                if (sensitive)
                {
                    configuration.AddTransformer(new CreditCardTransformer());
                    configuration.AddTransformer(new PhoneNumberTransformer());
                }

                if (obfuscate)
                {
                    configuration.AddTransformer(new UserObfuscateTransformer());
                }

                var exporter = new ConversationExporter(configuration);
                exporter.ExportConversation();
            }, new MediaTypeHeaderValue("application/json"));

            return response;
        }
    }
}
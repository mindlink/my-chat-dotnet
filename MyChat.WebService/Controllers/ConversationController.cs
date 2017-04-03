using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MindLink.MyChat.WebService.Controllers
{
    [RoutePrefix("conversation")]
    public class ConversationController : ApiController
    {
        [HttpPost, Route("")]
        public async Task<HttpResponseMessage> Post([FromBody]string value)
        {
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var file = await this.Request.Content.ReadAsMultipartAsync();
            

            return this.Request.CreateResponse(HttpStatusCode.Created, value);
        }
    }
}
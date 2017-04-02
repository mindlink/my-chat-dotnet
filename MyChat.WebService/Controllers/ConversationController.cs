using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyChat.WebService.Controllers
{
    [RoutePrefix("conversation")]
    public class ConversationController : ApiController
    {
        [HttpPost, Route("")]
        public HttpResponseMessage Post([FromBody]string value)
        {
            return this.Request.CreateResponse(HttpStatusCode.Created, value);
        }
    }
}
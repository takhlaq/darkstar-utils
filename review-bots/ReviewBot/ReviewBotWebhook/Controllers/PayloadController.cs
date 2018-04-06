using Microsoft.AspNetCore.Mvc;
using Octokit;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReviewBotWebhook.Controllers
{
    [Produces("application/json")]
    [Route("payload")]
    public class PayloadController : Controller
    {
        ReviewBot.Bot Bot = new ReviewBot.Bot();
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var json = await reader.ReadToEndAsync();
                if (json != null)
                {
                    var payload = new Octokit.Internal.SimpleJsonSerializer().Deserialize<PullRequestEventPayload>(json);
                    if (payload?.PullRequest?.State == ItemState.Open)
                    {
                        lock (Bot)
                        {
                            Bot.Queue(payload);
                        }
                    }
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
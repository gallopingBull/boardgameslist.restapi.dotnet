using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MYBGList_APIVersion.Controllers.v2
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeOnDemandController : ControllerBase
    {
        [EnableCors("AnyOrigin_GetOnly")]
        public void Get()
        {
            Results.Text("<script>" +
                "window.alert('Your client supports Javascript!" +
                "\\r\\n\\r\\n" +
                $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" +
                "\\r\\n" +
                "Client time (UTC): ' + new Date().toISOString());" +
                "</script>" +
                "<noscript>Your client does not support Javascript</noscript>", "text/html");
        }
    }
}

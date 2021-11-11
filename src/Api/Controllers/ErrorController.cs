using Core.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseController
    {
        public ErrorController(INotificador notificador) : base(notificador)
        {
        }

        [HttpGet("/error")]
        [HttpPost("/error")]
        public IActionResult PostError() => SetLocalError();

        private IActionResult SetLocalError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return ErrorResponse(context.Error.Message, context.Error.StackTrace);
        }
    }
}

using Business.Models;
using Core.Interfaces;
using Core.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using X.PagedList;

namespace Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly INotificador _notificador;
        public BaseController(INotificador notificador)
        {
            _notificador = notificador;
        }
        protected ActionResult CreatedResponse(string uri = "", object result = null)
        {
            if (OperacaoValida())
            {
                return Created(uri, result);
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected IActionResult UpdatedResponse()
        {
            return Ok();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(result);
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);
            return CustomResponse();
        }

        protected ActionResult CustomResponse<T>(IPagedList<T> list = null)
        {
            if (OperacaoValida())
                return Ok(new PagedResponse<T>()
                {
                    PageNumber = list.PageNumber,
                    TotalItens = list.TotalItemCount,
                    TotalPages = list.PageCount,
                    Data = list
                });

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected ActionResult ErrorResponse(string message = null, string stackTrace = null)
        {
            if (message != null)
                NotificarErro(message);

            return Problem(detail: stackTrace, title: message);
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMsg);
            }
        }

        protected void NotificarErro(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }
    }
}
using Business.Models;
using Core.Notificacoes;
using FluentValidation;
using FluentValidation.Results;

namespace Service
{
    public abstract class BaseService
    {
        public INotificador _notificador;

        public BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }
        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var item in validationResult.Errors)
            {
                Notificar(item.ErrorMessage);
            }
        }
        protected void Notificar(string message)
        {
            _notificador.Handle(new Notificacao(message));
        }
        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);

            return false;
        }
    }
}

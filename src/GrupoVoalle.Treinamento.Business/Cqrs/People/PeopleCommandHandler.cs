using GrupoVoalle.Base.Business.Primitives.People;
using GrupoVoalle.CrossCutting.Core.Bus;
using GrupoVoalle.CrossCutting.Core.Cqrs;
using GrupoVoalle.CrossCutting.Core.Cqrs.Notifications;
using GrupoVoalle.Domain.Core.Interfaces;
using GrupoVoalle.Utility.Messages;
using MediatR;

namespace GrupoVoalle.Base.Business.Cqrs.People
{
    public class PeopleCommandHandler :
        CommandHandler,
        IRequestHandler<PeopleCreateCommand, ResponseMessage>
    {
        private readonly IPeopleService _service;

        public PeopleCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IPeopleService service
        ) : base(uow, bus, notifications)
        {
            _service = service;
        }

        // realiza a chamada para o serviço de criação de pessoa na camada de domain
        public async Task<ResponseMessage> Handle(PeopleCreateCommand request, CancellationToken cancellationToken)
        {
            var resData = await _service.CreateAsync(request);

            var res = new ResponseMessage(resData);
            return res;
        }
    }
}
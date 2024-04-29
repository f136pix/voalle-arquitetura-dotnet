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
        IRequestHandler<PeopleCreateCommand, ResponseMessage>,
        IRequestHandler<PeopleUpdateCommand, ResponseMessage>,
        IRequestHandler<PeopleDeleteCommand, ResponseMessage>,
        IRequestHandler<PeopleGetCommand, ResponseMessage>,
        IRequestHandler<PeopleGetPagedCommand, ResponseMessage>
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
        // tem delegado para si o tratamento the PeopleCreateCommand
        public async Task<ResponseMessage> Handle(PeopleCreateCommand request, CancellationToken cancellationToken)
        {
            var retData = await _service.CreateAsync(request);

            var ret = new ResponseMessage(retData);
            return ret;
        }


        public async Task<ResponseMessage> Handle(PeopleUpdateCommand request, CancellationToken cancellationToken)
        {
            var retData = await _service.UpdateAsync(request);

            var ret = new ResponseMessage(retData);
            return ret;
        }

        public async Task<ResponseMessage> Handle(PeopleDeleteCommand request, CancellationToken cancellationToken)
        {
            var retData = await _service.DeleteAsync(request.Id);
            
            var ret = new ResponseMessage(retData);
            return ret;
        }

        public async Task<ResponseMessage> Handle(PeopleGetCommand request, CancellationToken cancellationToken)
        {
            var retData = await _service.GetAsync(request.Id);
            
            var ret = new ResponseMessage(retData);
            return ret;
        }
        
        public async Task<ResponseMessage> Handle(PeopleGetPagedCommand request, CancellationToken cancellationToken)
        {
            var retData = await _service.GetPagedAsync(request);
            
            var ret = new ResponseMessage(retData);
            return ret;
        }
    }
}
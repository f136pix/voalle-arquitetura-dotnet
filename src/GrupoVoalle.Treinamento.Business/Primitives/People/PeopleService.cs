using GrupoVoalle.Base.Business.Cqrs.People;
using GrupoVoalle.CrossCutting.Core.AutoMapper;
using GrupoVoalle.CrossCutting.Core.Bus;
using GrupoVoalle.Domain.Core.Interfaces;
using GrupoVoalle.Domain.Core.Interfaces.Services;
using GrupoVoalle.Domain.Core.Paging;
using GrupoVoalle.Domain.Core.Services;
using GrupoVoalle.Utility.CustomExceptions;
using GrupoVoalle.Utility.DynamicFields.Query;
using GrupoVoalle.Utility.Messages;
using GrupoVoalle.Utility.Security.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GrupoVoalle.Base.Business.Primitives.People
{
    #region Interface

    public interface IPeopleService : IService<PeopleEntity, long>
    {
        public Task<ReturnDataList> CreateAsync(PeopleCreateCommand model);

        public Task<ReturnDataList> GetPagedAsync(PeopleGetPagedCommand model);

        public Task<ReturnDataList> UpdateAsync(PeopleUpdateCommand model);
    }

    #endregion Interface

    #region Service

    public partial class PeopleService : Service<PeopleEntity, long>, IPeopleService
    {
        private readonly IServiceProvider _provider;
        private readonly IUnitOfWork _uow;
        private readonly UserHttp _userHttp;
        private readonly IMediatorHandler _bus;

        public PeopleService(
            IServiceProvider provider,
            IUnitOfWork uow,
            UserHttp userHttp,
            IMediatorHandler bus
        ) : base(provider.GetService<IPeopleRepository>())
        {
            _provider = provider;
            _uow = uow;
            _userHttp = userHttp;
            _bus = bus;
        }

        public async Task<ReturnDataList> CreateAsync(PeopleCreateCommand model)
        {
            ArgumentNullException.ThrowIfNull(model);

            // converts dto
            var domain = MappingData.Convert<PeopleCreateCommand, PeopleEntity>(model);
            
            domain.Created = DateTime.Now;
            domain.CreatedBy = _userHttp.Id;
            
            var res = await base.CreateAsync(domain);

            if (res.HasError())
                throw new BusinessException(res.GetMessages().Select(s => s.Message.Value).FirstOrDefault());
             
            await _uow.CommitAsync();

            return ReturnDataList.ToSuccess("People criado com sucesso");
        }

        public async Task<ReturnDataList> GetPagedAsync(PeopleGetPagedCommand model)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnDataList> UpdateAsync(PeopleUpdateCommand model)
        {
            throw new NotImplementedException();
        }
    }

    #endregion Service
}
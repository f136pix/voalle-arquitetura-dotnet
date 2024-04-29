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
        public void IsDelete();

        /// <summary>
        /// Cria um novo registro
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ReturnDataList> CreateAsync(PeopleCreateCommand model);

        /// <summary>
        /// Update por id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ReturnDataList> UpdateAsync(PeopleUpdateCommand model);

        /// <summary>
        /// Delete por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new Task<ReturnDataList> DeleteAsync(long id); // new -> overrides metodo herdado

        /// <summary>
        /// Busca paginada
        /// </summary>
        /// <param name="GetPeoplePagedCommand"></param>
        /// <returns></returns>
        Task<ReturnData<DataPage<object>>> GetPagedAsync(PeopleGetPagedCommand model);
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

            RegistryHooks();
        }

        // informa ao hook que a operação é de exclusão
        public void IsDelete()
        {
            // limpa o valor apos alguma operação ser concluida
            ClearExtraDataAfterPersist = true;
            // adiciona extra-data para o hook
            AddExtraData("IsDelete", true);
        }

        public async Task<ReturnDataList> CreateAsync(PeopleCreateCommand model)
        {
            ArgumentNullException.ThrowIfNull(model);

            // converts dto
            var domain = MappingData.Convert<PeopleCreateCommand, PeopleEntity>(model);

            domain.Created = DateTime.Now;
            domain.CreatedBy = _userHttp.Id;

            // acessa o repositório e cria o registro
            var ret = await base.CreateAsync(domain);

            if (ret.HasError())
                throw new BusinessException(ret.GetMessages().Select(s => s.Message.Value).FirstOrDefault());

            // aceesa o unit of work e commita alteração
            await _uow.CommitAsync();

            return ReturnDataList.ToSuccess("People criado com sucesso");
        }

        public async Task<ReturnDataList> UpdateAsync(PeopleUpdateCommand model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var filter = new FilterBy<PeopleEntity>(people => people.Id == model.Id);

            var domain = await QueryDb(filter)
                .FirstOrDefaultAsync();

            if (domain == null)
                throw new NotFoundException("Id não encontrado");

            if (domain.Deleted)
                throw new DeletedException();

            // disponibiliza model para o hook
            AddExtraData("Model", model);

            // traz os dados do updateCommand para dentro do domain
            //MappingData.Convert<PeopleUpdateCommand, PeopleEntity>(model);
            MappingData.Convert(
                origin: model,
                destiny: domain);

            domain.Modified = DateTime.Now;
            domain.ModifiedBy = _userHttp.Id;

            var ret = await base.UpdateAsync(domain);

            await _uow.CommitAsync();

            return ret;
            // ret_err == throw custom error
            // ret_ok == return ReturnDataList.ToSuccess("Operação realizada com sucesso");
        }

        // overrites metodo inherited
        public override async Task<ReturnDataList> DeleteAsync(long id)
        {
            var domain = await QueryDb(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (domain == null)
                throw new NotFoundException("Id não encontrado");

            if (domain.Deleted)
                throw new DeletedException();

            domain.Deleted = true;
            domain.ModifiedBy = _userHttp.Id;
            domain.Modified = DateTime.Now;

            IsDelete();

            await base.UpdateAsync(domain);

            await _uow.CommitAsync();

            return ReturnDataList.ToSuccess("Registro exluido com sucesso");
        }

        public async Task<ReturnData<DataPage<object>>> GetPagedAsync(PeopleGetPagedCommand model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var filter = new FilterBy<PeopleEntity>(model.Filter);
            filter.AddExpression(p => !p.Deleted);

            var data = await GetPageAnonymousAsync(
                startRow: CalcStartRow(model.Page, model.PageSize),
                pageSize: model.PageSize,
                select: new Select<PeopleEntity>(p => new
                {
                    p.Id,
                    p.Name,
                    p.Name2,
                    p.TypeTxId,
                    p.TxId
                }),
                filter: filter,
                orderBy: new OrderBy<PeopleEntity>(model.OrderBy)
            );
            var totalCount = await CountAsync(filter: filter);

            var dataPage = new DataPage<object>
            {
                Data = data,
                TotalRecords = totalCount,
                PageSize = model.PageSize,
                Page = model.Page
            };

            return new ReturnData<DataPage<object>>(true, dataPage);
        }
    }

    #endregion Service
}
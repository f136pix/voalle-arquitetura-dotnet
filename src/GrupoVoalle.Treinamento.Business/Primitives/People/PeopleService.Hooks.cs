using GrupoVoalle.Base.Business.Cqrs.People;
using GrupoVoalle.CrossCutting.Core.AutoMapper;
using GrupoVoalle.Domain.Core.Common.Enum;
using GrupoVoalle.Domain.Core.Services;
using GrupoVoalle.Treinamento.Business.Primitives.LogTables;
using GrupoVoalle.Utility.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GrupoVoalle.Base.Business.Primitives.People
{
    public partial class PeopleService
    {
        private void RegistryHooks()
        {
            AddHooksAsync(TypeHooks.AfterCreate, GenerateCreateLogAsync);
            AddHooksAsync(TypeHooks.Update, GenerateUpdateLogAsync);
            AddHooksAsync(TypeHooks.Update, GenerateDeleteLogAsync);
        }

        // log de criacao de people
        public async Task<ReturnData> GenerateCreateLogAsync(PeopleEntity entity, IDictionary<string, object> extra)
        {
            await _uow.CommitAsync();

            var logService = _provider.GetService<ILogTablesService>();

            var ret = await logService.PersistLog<PeopleEntity>(
                type: LogTablesTypeOperationEnum.Add,
                id: entity.Id.ToString()
            );

            return ret?.GetMessages().FirstOrDefault();
        }

        public async Task<ReturnData> GenerateUpdateLogAsync(PeopleEntity entity, IDictionary<string, object> extra)
        {
            var isDelete = extra.Any(w => w.Key == "IsDelete");
            if (isDelete)
                return ReturnData.ToSuccess("Não há edição, mas sim alteração de exclusão");

            var model = extra.FirstOrDefault(w => w.Key == "Model").Value as PeopleUpdateCommand;
            var domain = await QueryDb(p => p.Id == entity.Id).FirstOrDefaultAsync();

            var domainToCommand = MappingData.Convert<PeopleEntity, PeopleUpdateCommand>(domain);
            var diff = GenerateDiffLogs(
                model: model,
                domain: domainToCommand
            );

            var logService = _provider.GetService<ILogTablesService>();
            var ret = await logService.PersistLog<PeopleEntity>(
                type: LogTablesTypeOperationEnum.Edit,
                id: entity.Id.ToString(),
                logItems: diff
            );

            return ret?.GetMessages().FirstOrDefault();
        }

        public async Task<ReturnData> GenerateDeleteLogAsync(PeopleEntity entity, IDictionary<string, object> extra)
        {
            var isDelete = extra.Any(w => w.Key != "IsDelete");
            if (isDelete)
                return ReturnData.ToSuccess("Não há exclusão"); 

            var logService = _provider.GetService<ILogTablesService>();

            var ret = await logService.PersistLog<PeopleEntity>(
                type: LogTablesTypeOperationEnum.Delete,
                id: entity.Id.ToString()    
            );

            return ret?.GetMessages().FirstOrDefault();
        }
    }
}
using GrupoVoalle.CrossCutting.Core.AutoMapper;
using GrupoVoalle.Domain.Core.Common.Attributes;
using GrupoVoalle.Domain.Core.Common.Enum;
using GrupoVoalle.Domain.Core.Common.Model;
using GrupoVoalle.Domain.Core.Interfaces;
using GrupoVoalle.Domain.Core.Interfaces.Services;
using GrupoVoalle.Domain.Core.Services;
using GrupoVoalle.Treinamento.Business.Primitives.LogItems;
using GrupoVoalle.Utility.Extensions;
using GrupoVoalle.Utility.Messages;
using GrupoVoalle.Utility.Security.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GrupoVoalle.Treinamento.Business.Primitives.LogTables
{
    #region Interface
    public interface ILogTablesService : IService<LogTablesEntity, long>
    {
        /// <summary>
        ///  Realiza o registro dos logs de alteração de um cadastro
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="logItems"></param>
        Task<ReturnDataList> PersistLog<TEntity>(
            LogTablesTypeOperationEnum type,
            string id,
            List<LogItemsModel> logItems = null
        );

        /// <summary>
        /// Salva o registro de alteração de uma determinada entidade
        /// </summary>
        /// <param name="entity">Tipo da entidade</param>
        /// <param name="key">Identificador único do cadastro</param>
        /// <param name="operation">Tipo da ação</param>
        /// <param name="items">Items alterados</param>
        /// <returns></returns>
        Task<ReturnDataList> Save(
            Type entity,
            string key,
            LogTablesTypeOperationEnum operation,
            IList<LogItemsEntity> items = null
        );

        /// <summary>
        /// Salva o registro de alteração de uma determinada entidade
        /// </summary>
        /// <param name="type">Tipo da entidade</param>
        /// <param name="key">Identificador único do cadastro</param>
        /// <param name="operation">Tipo da ação</param>
        /// <param name="userId">Identificador do usuário</param>
        /// <param name="userName">Nome do usuário</param>
        /// <param name="userLogin">Login do usuário</param>
        /// <param name="personId">Id da pessoa</param>
        /// <param name="personName">Nome da pessoa</param>
        /// <param name="origin">Origem</param>
        /// <param name="items">Items alterados</param>
        /// <returns></returns>
        Task<ReturnDataList> Save(
            Type type,
            string key,
            LogTablesTypeOperationEnum operation,
            long userId,
            string userName,
            string userLogin,
            long personId,
            string personName,
            int origin,
            IList<LogItemsEntity> items = null
        );
    }
    #endregion Interface

    #region Service
    public class LogTablesService : Service<LogTablesEntity, long>, ILogTablesService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserHttp _user;
        private readonly IServiceProvider _provider;

        public LogTablesService(
            IServiceProvider provider,
            ILogTablesRepository repository,
            IUnitOfWork uow,
            UserHttp user
        ) : base(repository)
        {
            _provider = provider;
            _user = user;
            _uow = uow;
        }

        /// <summary>
        ///  Realiza o registro dos logs de alteração de um cadastro
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="logItems"></param>
        public async Task<ReturnDataList> PersistLog<TEntity>(
            LogTablesTypeOperationEnum type,
            string id,
            List<LogItemsModel> logItems = null
        )
        {
            if (type == LogTablesTypeOperationEnum.Edit && (logItems == null || logItems.Count == 0))
                return null;

            var logItemsService = _provider.GetService<ILogItemsService>();

            // Ajustas valores de FKs
            var fkItems = logItems?.Where(item => !string.IsNullOrEmpty(item.FkTableName)).ToList();
            if (fkItems != null)
            {
                foreach (var item in fkItems)
                {
                    // Busca o valor antigo
                    var oldFriendlyName = await logItemsService
                        .GetFriendlyNameAsync(
                            tableName: item.FkTableName,
                            columnName: item.FkColumnName.ToSnakeCase(),
                            columnId: item.FkColumnId,
                            idValue: item.OldValue
                        );

                    item.FriendlyOldValue = oldFriendlyName?.Name;

                    // Busca o valor novo
                    var newFriendlyName = await logItemsService
                        .GetFriendlyNameAsync(
                            tableName: item.FkTableName,
                            columnName: item.FkColumnName.ToSnakeCase(),
                            columnId: item.FkColumnId,
                            idValue: item.NewValue
                        );

                    item.FriendlyNewValue = newFriendlyName?.Name;
                }
            }

            var items = MappingData.Convert<LogItemsModel, LogItemsEntity>(logItems);

            return await Save(
                entity: typeof(TEntity),
                key: id,
                operation: type,
                items: items
            );
        }

        /// <summary>
        /// Salva o registro de alteração de uma determinada entidade
        /// </summary>
        /// <param name="entity">Tipo da entidade</param>
        /// <param name="key">Identificador único do cadastro</param>
        /// <param name="operation">Tipo da ação</param>
        /// <param name="items">Items alterados</param>
        /// <returns></returns>
        public async Task<ReturnDataList> Save(
            Type entity,
            string key,
            LogTablesTypeOperationEnum operation,
            IList<LogItemsEntity> items = null
        )
        {
            return await Save(
                type: entity,
                key: key,
                operation: operation,
                userId: _user.Id,
                userName: _user.Name,
                userLogin: _user.Login,
                personId: _user.PersonId ?? 1,
                personName: _user.PersonName,
                origin: _user.Origin,
                items: items
            );
        }

        /// <summary>
        /// Salva o registro de alteração de uma determinada entidade
        /// </summary>
        /// <param name="type">Tipo da entidade</param>
        /// <param name="key">Identificador único do cadastro</param>
        /// <param name="operation">Tipo da ação</param>
        /// <param name="userId">Identificador do usuário</param>
        /// <param name="userName">Nome do usuário</param>
        /// <param name="userLogin">Login do usuário</param>
        /// <param name="personId">Id da pessoa</param>
        /// <param name="personName">Nome da pessoa</param>
        /// <param name="origin">Origem</param>
        /// <param name="items">Items alterados</param>
        /// <returns></returns>
        public async Task<ReturnDataList> Save(
            Type type,
            string key,
            LogTablesTypeOperationEnum operation,
            long userId,
            string userName,
            string userLogin,
            long personId,
            string personName,
            int origin,
            IList<LogItemsEntity> items = null
        )
        {
            // Busca atributos da entidade que está sendo enviada
            var attribute = type.GetAttributeValue((LogTableAttribute dna) => dna);

            // Obtém a Assembly atual
            var assembly = Assembly.GetExecutingAssembly();

            // Obtém o atributo AssemblyTitleAttribute
            var titleAttribute = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute));

            // Realiza a montagem do banco
            var model = new LogTablesEntity
            {
                TransactionGuid = Guid.NewGuid(),
                Application = titleAttribute?.Title,
                DateLog = DateTime.Now,
                TableName = attribute != null
                    ? attribute.Name
                    : nameof(type),
                FriendlyTableName = attribute != null
                    ? attribute.FriendlyName
                    : nameof(type),
                RegisterKey = key,
                Operation = operation,
                Origin = origin,
                UserId = userId,
                UserName = userName,
                UserLogin = userLogin,
                PersonId = personId,
                PersonName = personName
            };

            // Se for edição, deverá salvar quais itens foram alterados
            if (operation == LogTablesTypeOperationEnum.Edit)
                model.LogItems = items;

            // Registra no banco de dados
            var messages = await CreateAsync(model);

            // Persiste as informações no banco
            await _uow.CommitAsync();

            return messages;
        }
    }
    #endregion Service
}
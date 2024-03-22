using GrupoVoalle.Domain.Core.Interfaces.Services;
using GrupoVoalle.Domain.Core.Services;
using GrupoVoalle.Treinamento.Business.Business.DTO.Functions;

namespace GrupoVoalle.Treinamento.Business.Primitives.LogItems
{
    #region Interface
    public interface ILogItemsService : IService<LogItemsEntity, long>
    {
        /// <summary>
        /// Retorna o valor amigável
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="columnId"></param>
        /// <param name="idValue"></param>
        /// <returns></returns>
        Task<FunctionFriendlyName> GetFriendlyNameAsync(
            string tableName,
            string columnName,
            string columnId,
            string idValue
        );
    }
    #endregion Interface

    #region Service
    public class LogItemsService : Service<LogItemsEntity, long>, ILogItemsService
    {
        private readonly ILogItemsRepository _repository;

        public LogItemsService(
            ILogItemsRepository repository
        ) : base(repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retorna o valor amigável
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="columnId"></param>
        /// <param name="idValue"></param>
        /// <returns></returns>
        public async Task<FunctionFriendlyName> GetFriendlyNameAsync(
            string tableName,
            string columnName,
            string columnId,
            string idValue
        )
        {
            return await _repository.GetFriendlyNameAsync(
                tableName: tableName,
                columnName: columnName,
                columnId: columnId,
                idValue: idValue
            );
        }
    }
    #endregion Service
}
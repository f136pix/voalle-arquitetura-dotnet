using GrupoVoalle.Domain.Core.Interfaces.Repositories;
using GrupoVoalle.Treinamento.Business.Business.DTO.Functions;

namespace GrupoVoalle.Treinamento.Business.Primitives.LogItems
{
    public interface ILogItemsRepository : IRepository<LogItemsEntity, long>
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
}
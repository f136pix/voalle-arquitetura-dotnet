using GrupoVoalle.Treinamento.Business.Business.DTO;
using GrupoVoalle.Treinamento.Business.Business.DTO.Functions;
using GrupoVoalle.Treinamento.Business.Primitives.LogItems;
using GrupoVoalle.Database.Core.Repositories;
using GrupoVoalle.Treinamento.Infra.DataContext;
using GrupoVoalle.Utility.Security.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GrupoVoalle.Treinamento.Infra.Persistence.Postgres.LogItems
{
    public class LogItemsRepository : Repository<GrupoVoalleContext, LogItemsEntity, long>, ILogItemsRepository
    {
        public LogItemsRepository(GrupoVoalleContext context, RecoveryStringConnection recovery, ILogger<LogItemsEntity> logger) : base(context, recovery, logger) { }

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
            return await _db.GetFriendlyName
                .FromSqlInterpolated($"SELECT * FROM vll_get_friendly_name({tableName}, {columnName}, {columnId}, {idValue})")
                .FirstOrDefaultAsync();
        }
    }
}
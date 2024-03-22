using GrupoVoalle.Treinamento.Business.Primitives.LogTables;
using GrupoVoalle.Database.Core.Repositories;
using GrupoVoalle.Treinamento.Infra.DataContext;
using GrupoVoalle.Utility.Security.Http;
using Microsoft.Extensions.Logging;

namespace GrupoVoalle.Treinamento.Infra.Persistence.Postgres.LogTables
{
    public class LogTablesRepository : Repository<GrupoVoalleContext, LogTablesEntity, long>, ILogTablesRepository
    {
        public LogTablesRepository(
            GrupoVoalleContext context,
            RecoveryStringConnection recovery,
            ILogger<LogTablesEntity> logger
        ) : base(context, recovery, logger)
        { }
    }
}
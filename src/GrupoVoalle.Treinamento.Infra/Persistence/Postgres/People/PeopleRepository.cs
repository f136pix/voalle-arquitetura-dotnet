using GrupoVoalle.Base.Business.Primitives.People;
using GrupoVoalle.Treinamento.Infra.DataContext;
using GrupoVoalle.Database.Core.Repositories;
using GrupoVoalle.Utility.Security.Http;
using Microsoft.Extensions.Logging;

namespace GrupoVoalle.Base.Infra.Persistence.Postgres.People
{
    public class PeopleRepository : Repository<GrupoVoalleContext, PeopleEntity, long>, IPeopleRepository
    {
        public PeopleRepository(GrupoVoalleContext context, RecoveryStringConnection recovery, ILogger<PeopleEntity> logger) : base(context, recovery, logger)
        {
        }
    }
}
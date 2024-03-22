using GrupoVoalle.CrossCutting.Core.Cqrs.Events;
using GrupoVoalle.Treinamento.Infra.EventSourcing;

namespace GrupoVoalle.Treinamento.Infra.MongoDb.EventSourcing
{
    public class EventStoreMongoDbRepository : IEventStoreRepository
    {
        public IList<StoredEvent> All(Guid aggregateId)
        {
            // TODO
            return null;
        }

        public void Store(StoredEvent theEvent)
        {
            // TODO
        }

        public void Dispose()
        {
            // TODO
        }
    }
}
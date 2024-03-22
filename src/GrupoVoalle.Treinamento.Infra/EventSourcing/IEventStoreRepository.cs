using GrupoVoalle.CrossCutting.Core.Cqrs.Events;

namespace GrupoVoalle.Treinamento.Infra.EventSourcing
{
    public interface IEventStoreRepository : IDisposable
    {
        void Store(StoredEvent theEvent);

        IList<StoredEvent> All(Guid aggregateId);
    }
}
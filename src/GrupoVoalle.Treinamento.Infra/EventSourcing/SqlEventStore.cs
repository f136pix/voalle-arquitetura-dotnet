using Newtonsoft.Json;
using GrupoVoalle.CrossCutting.Core.Cqrs.Events;

namespace GrupoVoalle.Treinamento.Infra.EventSourcing
{
    public class SqlEventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;

        public SqlEventStore(IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public void Save<T>(T theEvent) where T : Event
        {
            var serializedData = JsonConvert.SerializeObject(theEvent);
            var storedEvent = new StoredEvent(theEvent, serializedData, "");

            _eventStoreRepository.Store(storedEvent);
        }
    }
}
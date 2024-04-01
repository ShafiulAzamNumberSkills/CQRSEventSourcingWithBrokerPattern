using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.EventStore.Entities;

namespace CleanArchitecture.Infrastructure.Commands.IRepositories
{
    public interface IEventsRepository
    {
        Task<string> StoreEvent(Event eventData);
        Task<bool> CompleteEvent(string aggregateId);
    }
}

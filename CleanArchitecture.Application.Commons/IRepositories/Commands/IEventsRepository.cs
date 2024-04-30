using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.EventStore.Entities;

namespace CleanArchitecture.Application.Common.IRepositories.Commands
{
    public interface IEventsRepository
    {
        Task<string> StoreEvent(Event eventData);
        Task<bool> CompleteEvent(string aggregateId);
    }
}

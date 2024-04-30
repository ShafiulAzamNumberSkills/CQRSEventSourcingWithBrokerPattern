using CleanArchitecture.Application.Common.IRepositories.Commands;
using CleanArchitecture.Domain.EventStore.Entities;
using CleanArchitecture.Infrastructure.Common.Data.EventStore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Commands.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly EventContext _eventDBContext;
        public EventsRepository(EventContext context)
        {
            _eventDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CompleteEvent(string aggregateId)
        {
            try
            {
                Event eventData = _eventDBContext.Events.FirstOrDefault(r => r.AggregateId == aggregateId);
                eventData.IsCompleted = true;
                _eventDBContext.Entry(eventData).State = EntityState.Modified;
                await _eventDBContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {

                return false;

            }
        }


        public async Task<string> StoreEvent(Event eventData)
        {
            try
            {
                eventData.EventTime = DateTime.Now; 
                eventData.AggregateId = Guid.NewGuid().ToString();
                _eventDBContext.Events.Add(eventData);
                await _eventDBContext.SaveChangesAsync();
                return eventData.AggregateId;
            }
            catch (Exception ex)
            {

                return null;

            }
        }


      
    }
}

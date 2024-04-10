using AutoMapper;
using CleanArchitecture.Application.Common.IRepositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.Events.PostEvents.DeletePostEvent
{
    public class DeletePostEvent : INotification
    {
        public int Id { get; set; }
        public string AggregateId { get; set; }

    }

    public class DeletePostEventHandler : INotificationHandler<DeletePostEvent>
    {
        private readonly IEventsRepository _eventRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly ILogger<DeletePostEventHandler> _logger;

        public DeletePostEventHandler(IEventsRepository eventRepository,
                                      IPostsRepository postsRepository,
                                            ILogger<DeletePostEventHandler> logger)
        {
            _eventRepository = eventRepository;
            _postsRepository = postsRepository;
            _logger = logger;
        }

        public async Task Handle(DeletePostEvent deletePostEvent, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Delete Post Event: " + JsonConvert.SerializeObject(deletePostEvent));

                int ID = deletePostEvent.Id;    


                bool response = _postsRepository.DeletePost(ID);

                if (!response)
                {

                    // send error mail to user

                }
                else
                {
                    //send success mail to user
                }

                await _eventRepository.CompleteEvent(deletePostEvent.AggregateId);


            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                throw;

            }

        }
    }
}

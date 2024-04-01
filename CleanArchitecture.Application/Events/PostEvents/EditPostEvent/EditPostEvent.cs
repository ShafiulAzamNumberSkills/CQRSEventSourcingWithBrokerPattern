using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Commands.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.Events.PostEvents.EditPostEvent
{
    public class EditPostEvent : INotification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AggregateId { get; set; }
    }

    public class EditPostEventHandler : INotificationHandler<EditPostEvent>
    {
        private readonly IEventsRepository _eventRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EditPostEventHandler> _logger;

        public EditPostEventHandler(IEventsRepository eventRepository,
                                    IPostsRepository postsRepository,
                                            IMapper mapper,
                                            ILogger<EditPostEventHandler> logger)
        {
            _eventRepository = eventRepository;
            _postsRepository = postsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(EditPostEvent editPostEvent, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Edit Post Event: " + JsonConvert.SerializeObject(editPostEvent));

                Post objPost = _mapper.Map<Post>(editPostEvent);

                bool result = await _postsRepository.UpdatePost(objPost);

                if (!result)
                {

                    // send error mail to user

                }
                else { 
                
                    /// send success mail to user
                
                }

                await _eventRepository.CompleteEvent(editPostEvent.AggregateId);
                

            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                throw;

            }

        }
    }
}

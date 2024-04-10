using AutoMapper;
using CleanArchitecture.Application.Common.IRepositories.Commands;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.Commands.Events.PostEvents.CreatePostEvent
{
    public class CreatePostEvent : INotification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AggregateId { get; set; }
    }

    public class CreatePostEventHandler : INotificationHandler<CreatePostEvent>
    {
        private readonly IEventsRepository _eventRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePostEventHandler> _logger;

        public CreatePostEventHandler(IEventsRepository eventRepository,
                                            IPostsRepository postsRepository,
                                            IMapper mapper,
                                            ILogger<CreatePostEventHandler> logger)
        {
            _eventRepository = eventRepository;
            _postsRepository = postsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(CreatePostEvent createPostEvent, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Create Post Event: " + JsonConvert.SerializeObject(createPostEvent));

                Post post = _mapper.Map<Post>(createPostEvent);

                int id = await _postsRepository.InsertPost(post);

                if (id != 0)
                {
                    ///Send Confirmation Mail To User

                }
                else
                {
                    ///Send Error Information Mail To User
                }

                await _eventRepository.CompleteEvent(createPostEvent.AggregateId);

            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                throw;

            }

        }
    }
}

using AutoMapper;
using CleanArchitecture.Application.Commands.BrokerManager;
using CleanArchitecture.Application.Events.PostEvents.EditPostEvent;
using CleanArchitecture.Domain.EventStore.Entities;
using CleanArchitecture.Domain.ViewModels;
using CleanArchitecture.Infrastructure.Commands.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.Commands.Services.PostServices.EditPostCommand
{
    public class EditPostCommand : IRequest<ResponseModel>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class EditPostCommandHandler : IRequestHandler<EditPostCommand, ResponseModel>
    {
        private readonly IBrokerManager _brokerManager;
        private readonly IEventsRepository _eventRepository;
        private readonly Infrastructure.Queries.IRepositories.IPostsRepository _postsQueryRepository;
        private readonly ILogger<EditPostCommandHandler> _logger;
        private readonly IMapper _mapper;

        public EditPostCommandHandler(IBrokerManager brokerManager,
                                      IEventsRepository eventRepository,
                                      Infrastructure.Queries.IRepositories.IPostsRepository postsQueryRepository,
                                      ILogger<EditPostCommandHandler> logger,
                                      IMapper mapper)
        {
            _brokerManager = brokerManager;
            _eventRepository = eventRepository;
            _postsQueryRepository = postsQueryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseModel> Handle(EditPostCommand command, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Edit Post Command: " + JsonConvert.SerializeObject(command));


                var validationErrors = new Dictionary<string, string>();


                if (String.IsNullOrEmpty(command.Title))
                {
                    validationErrors["Title"] = "Title can not be empty.";
                    return ResponseModel.validationErrors(validationErrors);
                }

                var post = await _postsQueryRepository.GetPostByID(command.Id);

                if (post == null)
                {

                    return ResponseModel.customError("No Post Found!");

                }

                string id = await _eventRepository.StoreEvent(
                    new Event()
                    {
                        DataJson = JsonConvert.SerializeObject(command),
                        DataType = command.GetType().Name,
                    }
                    );

                if (id != null)
                {
                    EditPostEvent eventData = _mapper.Map<EditPostEvent>(command);
                    eventData.AggregateId = id;
                    _brokerManager.publish(eventData, eventData.GetType().Name);

                    return ResponseModel.ok(id);

                }
                else
                {
                    return ResponseModel.customError("Unexpected Error");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                throw;

            }

        }
    }
}

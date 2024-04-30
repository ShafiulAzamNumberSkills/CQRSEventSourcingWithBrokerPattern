using AutoMapper;
using CleanArchitecture.Application.Commands.BrokerManager;
using CleanArchitecture.Application.Commands.Events.PostEvents.CreatePostEvent;
using CleanArchitecture.Application.Common.IRepositories.Commands;
using CleanArchitecture.Domain.EventStore.Entities;
using CleanArchitecture.Domain.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.Commands.Services.PostServices.CreatePostCommand
{
    public class CreatePostCommand : IRequest<ResponseModel>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ResponseModel>
    {
        private readonly IEventsRepository _eventRepository;
        private readonly IBrokerManager _brokerManager;
        private readonly ILogger<CreatePostCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IBrokerManager brokerManager,
                                        IEventsRepository eventRepository,
                                        ILogger<CreatePostCommandHandler> logger,
                                        IMapper mapper)
        {
            _brokerManager = brokerManager;
            _eventRepository = eventRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseModel> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Create Post Command: " + JsonConvert.SerializeObject(command));


                var validationErrors = new Dictionary<string, string>();

                if (String.IsNullOrEmpty(command.Title))
                {
                    validationErrors["Title"] = "Title can not be empty.";
                    return ResponseModel.validationErrors(validationErrors);
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
                    CreatePostEvent eventData = _mapper.Map<CreatePostEvent>(command);
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

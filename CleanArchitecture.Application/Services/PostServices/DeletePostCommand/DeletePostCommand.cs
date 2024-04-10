using AutoMapper;
using CleanArchitecture.Application.Commands.BrokerManager;
using CleanArchitecture.Application.Common.IRepositories.Commands;
using CleanArchitecture.Application.Events.PostEvents.DeletePostEvent;
using CleanArchitecture.Domain.EventStore.Entities;
using CleanArchitecture.Domain.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.Commands.Services.PostServices.DeletePostCommand
{
    public class DeletePostCommand : IRequest<ResponseModel>
    {
        public int Id { get; set; }

    }

    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, ResponseModel>
    {
        private readonly IBrokerManager _brokerManager;
        private readonly IEventsRepository _eventRepository;
        private readonly Common.IRepositories.Queries.IPostsRepository _postsQueryRepository;
        private readonly ILogger<DeletePostCommandHandler> _logger;
        private readonly IMapper _mapper;

        public DeletePostCommandHandler(IBrokerManager brokerManager,
                                       IEventsRepository eventRepository,
                                       Common.IRepositories.Queries.IPostsRepository postsQueryRepository,
                                       ILogger<DeletePostCommandHandler> logger,
                                       IMapper mapper)
        {
            _brokerManager = brokerManager;
            _eventRepository = eventRepository;
            _postsQueryRepository = postsQueryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseModel> Handle(DeletePostCommand command, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Delete Post Command: " + JsonConvert.SerializeObject(command));

                int ID = command.Id;    

                var post = await _postsQueryRepository.GetPostByID(ID);

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

                    DeletePostEvent eventData = new DeletePostEvent() { Id = command.Id };
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

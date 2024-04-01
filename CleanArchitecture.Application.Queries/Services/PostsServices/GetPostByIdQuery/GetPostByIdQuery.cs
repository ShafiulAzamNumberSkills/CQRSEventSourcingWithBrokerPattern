using AutoMapper;
using CleanArchitecture.Domain.ViewModels;
using CleanArchitecture.Infrastructure.Queries.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.Queries.Services.PostServices.GetPostByIdQuery
{
    public class GetPostByIdQuery : IRequest<ResponseModel>
    {
        public int Id { get; set; }

    }

    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ResponseModel>
    {
        private readonly IPostsRepository _postsQueryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPostByIdQueryHandler> _logger;

        public GetPostByIdQueryHandler(IPostsRepository postsQueryRepository,
                                            IMapper mapper,
                                            ILogger<GetPostByIdQueryHandler> logger)
        {
            _postsQueryRepository = postsQueryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseModel> Handle(GetPostByIdQuery query, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Get Post By Id Query: " + JsonConvert.SerializeObject(query));

                var post = await _postsQueryRepository.GetPostByID(query.Id);

                if (post == null)
                {

                    return ResponseModel.customError("No Post Found!");

                }
                return ResponseModel.ok(post);

            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                throw;

            }

        }
    }
}

using AutoMapper;
using CleanArchitecture.Domain.ViewModels;
using CleanArchitecture.Infrastructure.Queries.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.Queries.Services.PostServices.GetPostsQuery
{
    public class GetPostsQuery : IRequest<ResponseModel>
    {
    }

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, ResponseModel>
    {
        private readonly IPostsRepository _postsQueryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPostsQueryHandler> _logger;

        public GetPostsQueryHandler(IPostsRepository postsQueryRepository,
                                            IMapper mapper,
                                            ILogger<GetPostsQueryHandler> logger)
        {
            _postsQueryRepository = postsQueryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseModel> Handle(GetPostsQuery query, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Get Posts Query: " + JsonConvert.SerializeObject(query));

                return ResponseModel.ok(await _postsQueryRepository.GetPosts());

            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                throw;

            }

        }
    }
}

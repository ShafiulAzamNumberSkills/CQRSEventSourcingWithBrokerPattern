using AutoMapper;
using CleanArchitecture.Application.Commands.Events.PostEvents.CreatePostEvent;

namespace CleanArchitecture.Application.Commands.Services.PostServices.CreatePostCommand
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreatePostCommand, CreatePostEvent>();
        }
    }
}

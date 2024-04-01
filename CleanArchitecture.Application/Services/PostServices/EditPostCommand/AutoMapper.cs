using AutoMapper;
using CleanArchitecture.Application.Events.PostEvents.EditPostEvent;

namespace CleanArchitecture.Application.Commands.Services.PostServices.EditPostCommand
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EditPostCommand, EditPostEvent>();
        }
    }
}

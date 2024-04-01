using CleanArchitecture.Application.Commands.Services.PostServices.CreatePostCommand;
using CleanArchitecture.Application.Commands.Services.PostServices.DeletePostCommand;
using CleanArchitecture.Application.Commands.Services.PostServices.EditPostCommand;
using CleanArchitecture.Application.Queries.Services.PostServices.GetPostByIdQuery;
using CleanArchitecture.Application.Queries.Services.PostServices.GetPostsQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("GetPost")]
        public async Task<IActionResult> GetPosts()
        {
            return Ok(await _mediator.Send(new GetPostsQuery()));
        }
        [HttpGet]
        [Route("GetPostByID/{Id}")]
        public async Task<IActionResult> GetPostByID(int Id)
        {
            return Ok(await _mediator.Send(new GetPostByIdQuery() { Id = Id }));
        }
        [HttpPost]
        [Route("AddPost")]
        public async Task<IActionResult> AddPost(CreatePostCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPut]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost(EditPostCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost(int id)
        {
            return Ok(await _mediator.Send(new DeletePostCommand() { Id = id}));
        }
    }
}

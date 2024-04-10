using CleanArchitecture.Application.Common.IRepositories.Queries;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Common.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Queries.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly PostContext _postDBContext;
        public PostsRepository(PostContext context)
        {
            _postDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _postDBContext.Posts.ToListAsync();
        }
        public async Task<Post> GetPostByID(int ID)
        {
            return await _postDBContext.Posts.FindAsync(ID);
        }
       
    }
}

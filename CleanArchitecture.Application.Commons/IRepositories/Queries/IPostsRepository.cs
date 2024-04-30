using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.IRepositories.Queries
{
    public interface IPostsRepository
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<Post> GetPostByID(int ID);
    }
}

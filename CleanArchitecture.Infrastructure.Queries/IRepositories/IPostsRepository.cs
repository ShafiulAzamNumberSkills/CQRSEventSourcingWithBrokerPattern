using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Infrastructure.Queries.IRepositories
{
    public interface IPostsRepository
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<Post> GetPostByID(int ID);
    }
}

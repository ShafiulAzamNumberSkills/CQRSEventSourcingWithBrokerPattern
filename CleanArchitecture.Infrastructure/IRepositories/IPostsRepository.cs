using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Infrastructure.Commands.IRepositories
{
    public interface IPostsRepository
    {
        Task<int> InsertPost(Post objPost);
        Task<bool> UpdatePost(Post objPost);
        bool DeletePost(int ID);
    }
}

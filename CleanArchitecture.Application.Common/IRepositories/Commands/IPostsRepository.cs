using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.IRepositories.Commands
{
    public interface IPostsRepository
    {
        Task<int> InsertPost(Post objPost);
        Task<bool> UpdatePost(Post objPost);
        bool DeletePost(int ID);
    }
}

using CleanArchitecture.Application.Common.IRepositories.Commands;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Common.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Commands.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly PostContext _postDBContext;
        public PostsRepository(PostContext context)
        {
            _postDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> InsertPost(Post objPost)
        {
            try
            {
                _postDBContext.Posts.Add(objPost);
                await _postDBContext.SaveChangesAsync();
                return objPost.Id;
            }
            catch (Exception ex) { 
            
                return 0;
            
            }
        }
        public async Task<bool> UpdatePost(Post objPost)
        {
            try
            {
                Post post = _postDBContext.Posts.Find(objPost.Id);
                post.Title = objPost.Title;
                post.Description = objPost.Description;
                _postDBContext.Entry(post).State = EntityState.Modified;
                await _postDBContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex) {

                return false;

            }

        }
        public bool DeletePost(int ID)
        {
            try
            {
                _postDBContext.Posts.Remove(_postDBContext.Posts.Find(ID));
                _postDBContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

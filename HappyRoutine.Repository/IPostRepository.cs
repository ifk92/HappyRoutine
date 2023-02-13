using HappyRoutine.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyRoutine.Repository
{
    public interface IPostRepository
    {
        public Task<Post> UpsertAsync(PostCreateDto postCreate, int applicationUserId);
        public Task<PagedResults<Post>> GetAllAsync(PostPaging postPaging);
        public Task<Post> GetAsync(int postId);
        public Task<int> DeleteAsync(int postId);
        public Task<List<Post>> GetAllByUserIdAsync(int applicationUserId);
        public Task<List<Post>> GetAllFamousAsync();
    }
}

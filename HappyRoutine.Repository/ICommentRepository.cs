using HappyRoutine.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyRoutine.Repository
{
    public interface ICommentRepository
    {
        public Task<Comment> UpsertAsync(CommentCreate commentCreate, int applicationUserId);
        public Task<Comment> GetAsync(int commentId);
        public Task<int> DeleteAsync(int commentId);
        public Task<List<Comment>> GetAllAsync(int postId);
    }
}

using HappyRoutine.Models.Comment;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace HappyRoutine.Repository
{
    public class CommentRepository:ICommentRepository
    {
        private readonly IConfiguration _config;

        public CommentRepository(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<int> DeleteAsync(int commentId)
        {
            int affectedRows = 0;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(
                    "Comment_Delete",
                    new { CommentId = commentId },
                    commandType: CommandType.StoredProcedure);
            }

            return affectedRows;
        }

        public async Task<List<Comment>> GetAllAsync(int postId)
        {
            IEnumerable<Comment> comments;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                comments = await connection.QueryAsync<Comment>(
                    "Comment_GetAll",
                    new { PostId = postId },
                    commandType: CommandType.StoredProcedure);
            }

            return comments.ToList();
        }

        public async Task<Comment> GetAsync(int commentId)
        {
            Comment comment;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                comment = await connection.QueryFirstOrDefaultAsync<Comment>(
                    "Comment_Get",
                    new { CommentId = commentId },
                    commandType: CommandType.StoredProcedure);
            }

            return comment;
        }

        public async Task<Comment> UpsertAsync(CommentCreate commentCreate, int applicationUserId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("CommentId", typeof(int));
            dataTable.Columns.Add("ParentCommentId", typeof(int));
            dataTable.Columns.Add("PostId", typeof(int));
            dataTable.Columns.Add("Content", typeof(string));

            dataTable.Rows.Add(
                commentCreate.CommentId,
                commentCreate.ParentCommentId,
                commentCreate.PostId,
                commentCreate.Content);

            int? newCommentId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newCommentId = await connection.ExecuteScalarAsync<int?>(
                    "Comment_Upsert",
                    new
                    {
                        Comment = dataTable.AsTableValuedParameter("dbo.CommentType"),
                        ApplicationUserId = applicationUserId
                    },
                    commandType: CommandType.StoredProcedure);
            }

            newCommentId = newCommentId ?? commentCreate.CommentId;

            Comment Comment = await GetAsync(newCommentId.Value);

            return Comment;
        }
    }
}

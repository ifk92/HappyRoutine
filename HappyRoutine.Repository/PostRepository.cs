using HappyRoutine.Models.Post;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Reflection.Metadata;

namespace HappyRoutine.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly IConfiguration _config;

        public PostRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int postId)
        {
            int affectedRows = 0;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(
                    "Post_Delete",
                    new { BlogId = postId },
                    commandType: CommandType.StoredProcedure);
            }

            return affectedRows;
        }

        public async Task<PagedResults<Post>> GetAllAsync(PostPaging postPaging)
        {
            var results = new PagedResults<Post>();

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync("Post_GetAll",
                    new
                    {
                        Offset = (postPaging.Page - 1) * postPaging.PageSize,
                        PageSize = postPaging.PageSize
                    },
                    commandType: CommandType.StoredProcedure))
                {
                    results.Items = multi.Read<Post>();

                    results.TotalCount = multi.ReadFirst<int>();
                }
            }

            return results;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            IEnumerable<Post> posts;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                posts = await connection.QueryAsync<Post>(
                    "Post_GetAllFamous",
                    new { },
                    commandType: CommandType.StoredProcedure);
            }

            return posts.ToList();
        }

        public async Task<List<Post>> GetAllByUserIdAsync(int applicationUserId)
        {
            IEnumerable<Post> posts;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                posts = await connection.QueryAsync<Post>(
                    "Post_GetByUserId",
                    new { ApplicationUserId = applicationUserId },
                    commandType: CommandType.StoredProcedure);
            }

            return posts.ToList();
        }

        public async Task<List<Post>> GetAllFamousAsync()
        {
            IEnumerable<Post> famousPosts;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                famousPosts = await connection.QueryAsync<Post>(
                    "Post_GetAllFamous",
                    new { },
                    commandType: CommandType.StoredProcedure);
            }

            return famousPosts.ToList();
        }

        public async Task<Post> GetAsync(int postId)
        {
            Post post;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                post = await connection.QueryFirstOrDefaultAsync<Post>(
                    "Post_Get",
                    new { PostId = postId },
                    commandType: CommandType.StoredProcedure);
            }
            
            return post;
        }

        public async Task<Post> UpsertAsync(PostCreate postCreate, int applicationUserId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PostId", typeof(int));
            dataTable.Columns.Add("Content", typeof(string));

            dataTable.Rows.Add(postCreate.PostId, postCreate.Content);

            int? newPostId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newPostId = await connection.ExecuteScalarAsync<int?>(
                    "Post_Upsert",
                    new { Post = dataTable.AsTableValuedParameter("dbo.PostType"), ApplicationUserId = applicationUserId },
                    commandType: CommandType.StoredProcedure
                    );
            }

            newPostId = newPostId ?? postCreate.PostId;

            Post post = await GetAsync(newPostId.Value);

            return post;
        }
    }
}

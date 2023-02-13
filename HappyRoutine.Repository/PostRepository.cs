using Dapper;
using HappyRoutine.Models.Post;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HappyRoutine.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly IConfiguration _config;

        public PostRepository(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<int> DeleteAsync(int postId)
        {
            using (var connection = await OpenConnectionAsync())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var deletedLineCount = await DeletePost(connection, postId, transaction);
                    transaction.Commit();
                    return deletedLineCount;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<PagedResults<Post>> GetAllAsync(PostPaging postPaging)
        {
            if (postPaging == null)
                throw new ArgumentNullException(nameof(postPaging));

            var results = new PagedResults<Post>();
            var offset = (postPaging.Page - 1) * postPaging.PageSize;

            using (var connection = await OpenConnectionAsync())
            using (var multi = await connection.QueryMultipleAsync(
                    "Post_GetAll",
                    new { Offset = offset, PageSize = postPaging.PageSize },
                    commandType: CommandType.StoredProcedure))
            {
                results.Items = multi.Read<Post>();
                results.TotalCount = multi.ReadFirst<int>();
            }

            return results;
        }


        public async Task<List<Post>> GetAllAsync()
        {
            IEnumerable<Post> posts;

            using (var connection = await OpenConnectionAsync())
            {
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

            using (var connection = await OpenConnectionAsync())
            {
                posts = await connection.QueryAsync<Post>(
                    "Post_GetByUserId",
                    new { ApplicationUserId = applicationUserId },
                    commandType: CommandType.StoredProcedure);
            }

            return posts.ToList();
        }


        public async Task<List<Post>> GetAllFamousAsync()
        {
            var famousPosts = new List<Post>();

            using (var connection = await OpenConnectionAsync())
            {
                await connection.OpenAsync();

                var queryResult = await connection.QueryAsync<Post>(
                    "Post_GetAllFamous",
                    commandType: CommandType.StoredProcedure);

                famousPosts.AddRange(queryResult);
            }

            return famousPosts;
        }


        public async Task<Post> GetAsync(int postId)
        {
            Post post;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                post = await connection.QueryFirstOrDefaultAsync<Post>(
                    "Post_Get",
                    new { PostId = postId },
                    commandType: CommandType.StoredProcedure);
            }

            return post;
        }

        public async Task<Post> UpsertAsync(PostCreateDto postCreationData, int userId)
        {
            if (postCreationData == null)
                throw new ArgumentNullException(nameof(postCreationData));

            int createdPostId = await ExecutePostCreationAsync(postCreationData, userId);
            Post createdPost = await GetPostAsync(createdPostId);

            return createdPost;
        }

        private async Task<int> ExecutePostCreationAsync(PostCreateDto postCreationData, int userId)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                return await connection.ExecuteScalarAsync<int>(
                    "Post_Upsert",
                    new { Post = CreateDataTable(postCreationData).AsTableValuedParameter("dbo.PostType"), ApplicationUserId = userId },
                    commandType: CommandType.StoredProcedure
                    );
            }
        }

        private DataTable CreateDataTable(PostCreateDto postCreationData)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PostId", typeof(int));
            dataTable.Columns.Add("Content", typeof(string));

            dataTable.Rows.Add(postCreationData.PostId, postCreationData.Content);

            return dataTable;
        }

        private async Task<Post> GetPostAsync(int postId)
        {
            return await GetAsync(postId);
        }


        private async Task<SqlConnection> OpenConnectionAsync()
        {
            var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            return connection;
        }

        private async Task<int> DeletePost(IDbConnection connection, int postId, IDbTransaction transaction)
        {
            var sql = "Post_Delete";
            var parameters = new { BlogId = postId };
            var commandType = CommandType.StoredProcedure;

            return await connection.ExecuteAsync(sql, parameters, transaction, null, commandType);
        }




    }
}

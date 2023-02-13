using HappyRoutine.Models.Post;
using HappyRoutine.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HappyRoutine.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        // TODO Mapping & Logger
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Post>> Create(PostCreateDto postCreateDto)
        {
            if (postCreateDto == null)
            {
                return BadRequest();
            }

            int applicationUserId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var post = await _postRepository.UpsertAsync(postCreateDto, applicationUserId);

            return Ok(post);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<Post>>> GetAll([FromQuery] PostPaging postPaging)
        {
            try
            {
                var posts = await _postRepository.GetAllAsync(postPaging);

                return Ok(posts);
            }
            catch (Exception ex)
            {

                return BadRequest("Failed to get posts");
            }

        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<Post>> Get(int postId)
        {

            try
            {
                var post = await _postRepository.GetAsync(postId);

                if (post == null)
                {
                    return BadRequest("Post not found");
                }
                return Ok(post);

            }
            catch (Exception ex)
            {

                return BadRequest("Failed to get post");
            }

        }

        [HttpGet("user/{applicationUserId}")]
        public async Task<ActionResult<List<Post>>> GetByApplicationUserId(int applicationUserId)
        {
            var posts = await _postRepository.GetAllByUserIdAsync(applicationUserId);

            return Ok(posts);
        }

        [HttpGet("famous")]
        public async Task<ActionResult<List<Post>>> GetAllFamous()
        {
            var posts = await _postRepository.GetAllFamousAsync();

            return Ok(posts);
        }

        [Authorize]
        [HttpDelete("{postId}")]
        public async Task<ActionResult<int>> Delete(int postId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var foundPost = await _postRepository.GetAsync(postId);

            if (foundPost == null) return NotFound("Post not found.");

            if (foundPost.ApplicationUserId != applicationUserId)
            {
                return Unauthorized("You don't have permission to delete this post.");
            }

            var affectedRows = await _postRepository.DeleteAsync(postId);

            return Ok(affectedRows);
        }

    }
}

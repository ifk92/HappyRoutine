using HappyRoutine.Models.Account;
using HappyRoutine.Models.Post;
using HappyRoutine.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace HappyRoutine.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<Post>> Create(PostCreate postCreate)
        {

            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var post = await _postRepository.UpsertAsync(postCreate, applicationUserId);

            return Ok(post);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<Post>>> GetAll([FromQuery] PostPaging postPaging)
        {
            var posts = await _postRepository.GetAllAsync(postPaging);

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<Post>> Get(int postId)
        {
            var post = await _postRepository.GetAsync(postId);

            return Ok(post);
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

            if (foundPost == null) return BadRequest("Post does not exist.");

            if (foundPost.ApplicationUserId == applicationUserId)
            {
                var affectedRows = await _postRepository.DeleteAsync(postId);

                return Ok(affectedRows);
            }
            else
            {
                return BadRequest("You didn't create this post.");
            }
        }
    }
}

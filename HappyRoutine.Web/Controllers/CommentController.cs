using HappyRoutine.Models.Comment;
using HappyRoutine.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace HappyRoutine.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Comment>> Create(CommentCreate commentCreate)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var createdComment = await _commentRepository.UpsertAsync(commentCreate, applicationUserId);

            return Ok(createdComment);
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<List<Comment>>> GetAll(int postId)
        {
            var postComments = await _commentRepository.GetAllAsync(postId);

            return postComments;
        }

        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<ActionResult<int>> Delete(int commentId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var foundComment = await _commentRepository.GetAsync(commentId);

            if (foundComment == null) return BadRequest("Comment does not exist.");

            if (foundComment.ApplicationUserId == applicationUserId)
            {
                var affectedRows = await _commentRepository.DeleteAsync(commentId);

                return Ok(affectedRows);
            }
            else
            {
                return BadRequest("This comment was not created by the current user.");
            }
        }
    }
}

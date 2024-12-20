using CommentsAPI.DTO;
using CommentsAPI.Interface;
using CommentsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentToCommentController : ControllerBase
    {
        ICommentToCommentService _commentService;

        public CommentToCommentController(ICommentToCommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("CreateCommentToComment:{commentParentId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateCommentToComment(string commentParentId, [FromBody] CommentToCommentReceived commentReceived)
        {
            try
            {
                CommentToComment comment = commentReceived.TurnIntoModel();
                comment = await _commentService.AddCommentToComment(commentParentId, comment);
                CommentToComment found = await _commentService.GetCommentOfCommentFromId(comment.Id);
                if (found == null)
                {
                    return BadRequest("Comment could not be created");
                }

                return Ok(comment);
            }
            catch (Exception)
            {
                return BadRequest("Comment could not be found");
            }
        }

        [HttpGet("GetCommentOfComment:{commentId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCommentOfCommentFromId(string commentId)
        {
            CommentToComment comment = await _commentService.GetCommentOfCommentFromId(commentId);
            if (comment == null)
            {
                return NotFound("Comment could not be found");
            }

            return Ok(comment);
        }

        [HttpGet("GetCommentOfCommentFromUserId:{userId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCommentsToCommentsFromUserId(string userId)
        {
            List<CommentToComment> comment = await _commentService.GetCommentsToCommentsFromUserId(userId);
            if (comment == null)
            {
                return NotFound("The provided userId doesn't have a comment");
            }

            return Ok(comment);
        }

        [HttpPut("UpdateCommentText:{commentId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateCommentOfCommentText(string commentId, string text)
        {
            CommentToComment comment = await _commentService.UpdateCommentOfCommentText(commentId, text);
            if (comment == null)
            {
                return NotFound("The provided commentId doesn't exist");
            }

            return Ok(comment);
        }

        [HttpDelete("Owner:{ownerId}/DeleteCommentText:{commentId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteCommentTextById(string commentId, string ownerId)
        {
            bool deleted = await _commentService.DeleteComment(commentId, ownerId);
            if (!deleted)
            {
                return BadRequest("Could not delete the given commentId");
            }

            return Ok(deleted);
        }
    }
}

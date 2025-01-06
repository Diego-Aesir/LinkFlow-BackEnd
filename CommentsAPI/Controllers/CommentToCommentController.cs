using CommentsAPI.DTO;
using CommentsAPI.Interface;
using CommentsAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CommentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("SercurityPolicy")]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCommentOfCommentFromId(string commentId)
        {
            try
            {
                CommentToComment comment = await _commentService.GetCommentOfCommentFromId(commentId);
                if (comment == null)
                {
                    return NotFound("Comment could not be found");
                }

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCommentOfCommentFromUserId:{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCommentsToCommentsFromUserId(string userId)
        {
            try
            {
                List<CommentToComment> comment = await _commentService.GetCommentsToCommentsFromUserId(userId);
                if (comment == null)
                {
                    return NotFound("The provided userId doesn't have a comment");
                }

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateCommentText:{commentId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateCommentOfCommentText(string commentId, [FromBody] string text)
        {
            try
            {
                CommentToComment comment = await _commentService.UpdateCommentOfCommentText(commentId, text);
                if (comment == null)
                {
                    return NotFound("The provided commentId doesn't exist");
                }

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Owner:{ownerId}/DeleteCommentText:{commentId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteCommentTextById(string commentId, string ownerId)
        {
            try
            {
                bool deleted = await _commentService.DeleteComment(commentId, ownerId);
                if (!deleted)
                {
                    return BadRequest("Could not delete the given commentId");
                }

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

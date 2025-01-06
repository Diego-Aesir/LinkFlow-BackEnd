using LinkFlowWebBFF.DTO;
using LinkFlowWebBFF.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkFlowWebBFF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentToCommentController : ControllerBase
    {
        public readonly ApiService _apiService;

        public CommentToCommentController(ApiService apiService) 
        {
            _apiService = apiService;
        }

        [HttpPost("create/parentComment:{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateCommentToComment(string commentId, [FromBody] CommentsToComments commentToComment)
        {
            try
            {
                var response = await _apiService.createCommentToComment(commentId, commentToComment);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCommentToCommentFromId(string commentId)
        {
            try
            {
                var response = await _apiService.GetCommentToCommentFromId(commentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCommentToCommentsFromUserId(string userId)
        {
            try
            {
                var response = await _apiService.GetCommentToCommentsFromUserId(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateCommentOfCommentText(string commentId, [FromBody] string text)
        {
            try
            {
                var response = await _apiService.UpdateCommentOfCommentText(commentId, text);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/ownerId:{ownerId}/{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> DeleteCommentToCommentTextById(string commentId, string ownerId)
        {
            try
            {
                var response = await _apiService.DeleteCommentToCommentTextById(commentId, ownerId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

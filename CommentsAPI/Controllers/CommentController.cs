﻿using CommentsAPI.Interface;
using Microsoft.AspNetCore.Mvc;
using CommentsAPI.Model;
using CommentsAPI.DTO;

namespace CommentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        public readonly ICommentService _commentService;
        public CommentController(ICommentService commentService) 
        {
            _commentService = commentService;
        }

        [HttpPost("CreateComment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateComment([FromBody] CommentReceived commentReceived)
        {
            Comment comment = await _commentService.CreateComment(commentReceived.createCommentModel());
            if (comment == null || comment.Id == null)
            {
                return BadRequest("Could not create this comment");
            }

            return Ok(comment);
        }

        [HttpGet("GetCommentFromCommentId:{commentId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentById(string commentId)
        {
            Comment found = await _commentService.GetCommentById(commentId);
            if (found == null)
            {
                return NotFound("Comment with given Id wasn't found");
            }

            return Ok(found);
        }

        [HttpGet("GetCommentsIdFromPostId:{postId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentByPostId(string postId)
        {
            List<Comment> list = await _commentService.GetCommentsFromPost(postId);
            if (list == null)
            {
                return NotFound("Comments Id could not be found");
            }

            return Ok(list);
        }

        [HttpGet("GetCommentsIdFromUser:{userId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentByUserId(string userId)
        {
            List<Comment> list = await _commentService.GetCommentsFromUser(userId);
            if (list == null)
            {
                return NotFound("Comments Id could not be found");
            }

            return Ok(list);
        }

        [HttpPut("UpdateCommentText:{commentId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateCommentText(string commentId, [FromBody] string commentText)
        {
            try
            {
                Comment updatedComment = await _commentService.UpdateCommentText(commentId, commentText);
                if (updatedComment.Text != commentText)
                {
                    return BadRequest("Comment Text was not updated");
                }

                return Ok(updatedComment);
            }
            catch (Exception)
            {
                return NotFound("Couldn't find this comment to update");
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
                    return BadRequest("Could not delete this comment");
                }

                return Ok(deleted);
            }
            catch (Exception)
            {
                return BadRequest("Either the owner id doesn't match with the comment owner or the comment could not be found");
            }
        }

    }
}
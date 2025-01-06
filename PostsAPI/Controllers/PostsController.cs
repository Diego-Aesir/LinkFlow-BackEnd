using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PostsAPI.DTO;
using PostsAPI.Interface;
using PostsAPI.Models;

namespace PostsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("SecurityPolicy")]
    public class PostsController : ControllerBase
    {
        IPostCommands _postCommands;
        public PostsController(IPostCommands postCommands)
        {
            _postCommands = postCommands;
        }

        [HttpPost("createPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePost([FromBody] PostsDTO newPost)
        {
            try
            {
                if (newPost == null)
                {
                    return BadRequest("New Post information wasn't sent");
                }
                Posts createPost = newPost.ToPosts();
                return Ok(await _postCommands.CreatePost(createPost));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{postId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPost(string postId)
        {
            try
            {
                Posts post = await _postCommands.GetPostAsync(postId);
                if (post == null)
                {
                    return NotFound($"This Post couldn't be found {postId}");
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("recentPosts/{page}/{limit}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetRecentPosts(int page, int limit)
        {
            try
            {
                List<Posts> list = new List<Posts>(await _postCommands.GetRecentPosts(page, limit));
                if (list == null)
                {
                    return NotFound("Could not find a list within this number of page or limit");
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("recentPosts/FromTags/{page}/{limit}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetRecentPostsFromTags(int page, int limit, [FromQuery] List<string> tags)
        {
            try
            {
                if (tags == null)
                {
                    return BadRequest("Can't continue without tags");
                }

                List<Posts> list = new List<Posts>(await _postCommands.GetPostsFromTags(tags, page, limit));
                if (list == null)
                {
                    return NotFound("Could not find a list from those tags with this number of pages or limits");
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("recentPosts/FromUser:{userId}/{page}/{limit}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetRecentPostsFromUser(int page, int limit, string userId)
        {
            try
            {
                List<Posts> list = new List<Posts>(await _postCommands.GetPostsAsyncByOwner(userId, page, limit));
                if (list == null)
                {
                    return NotFound($"Could not find a list from this user: {userId} with this number of pages or limits");
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("recentPosts/FromTitle/{page}/{limit}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetRecentPostsFromTitle(int page, int limit, [FromQuery] List<string> titles)
        {
            try
            {
                if (titles == null)
                {
                    return BadRequest("A list of Titles are required");
                }

                List<Posts> list = new List<Posts>(await _postCommands.GetPostsFromTitle(titles, page, limit));
                if (list == null)
                {
                    return NotFound($"Could not find a list from those titles: {titles} with this number of pages or limits");
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updatePost/{ownerId}/{postId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdatePost(string ownerId, string postId, [FromBody] PostsDTO post)
        {
            try
            {
                if (post == null)
                {
                    return BadRequest("Couldn't continue without a new post information");
                }

                Posts updatedPost = await _postCommands.UpdatePostAsync(postId, ownerId, post);
                if (updatedPost == null)
                {
                    return BadRequest("Post couldn't be found");
                }

                return Ok(updatedPost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deletePost/{userId}/{postId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeletePost(string userId, string postId)
        {
            try
            {
                bool deleted = await _postCommands.DeletePostAsync(postId, userId);
                if (deleted == false)
                {
                    return BadRequest("Post couldn't be deleted");
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

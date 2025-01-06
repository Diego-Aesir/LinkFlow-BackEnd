using Microsoft.AspNetCore.Mvc;
using LinkFlowWebBFF.DTO;
using LinkFlowWebBFF.Services;
using Microsoft.AspNetCore.Authorization;

namespace LinkFlowWebBFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApiService _apiService;

        public PostsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> createPost([FromBody] Posts post)
        {
            try
            {
                var response = await _apiService.CreatePost(post);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPost(string postId)
        {
            try
            {
                var response = await _apiService.GetPost(postId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("recent/{page}/{limit}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHomePagePostsWithoutTags(int page, int limit)
        {
            try
            {
                var response = await _apiService.GetRecentPostsWithoutTag(page, limit);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("recent/tags/{page}/{limit}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHomePagePostsWithTags(int page, int limit, [FromQuery] List<string> tags)
        {
            try
            {
                var response = await _apiService.GetRecentPostsWithTags(page, limit, tags);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("recent/user:{userId}/{page}/{limit}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRecentPostsFromUser(string userId, int page, int limit)
        {
            try
            {
                var response = await _apiService.GetRecentPostsFromUser(page, limit, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("recent/titles/{page}/{limit}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRecentPagePostsWithTitles(int page, int limit, [FromQuery] List<string> titles)
        {
            try
            {
                var response = await _apiService.GetRecentPostsFromTitle(page, limit, titles);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpPut("update/{userId}/{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdatePost(string userId, string postId, [FromBody] Posts updatedPost)
        {
            try
            {
                var response = await _apiService.UpdatePost(userId, postId, updatedPost);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }

        }

        [HttpDelete("delete/{userId}/{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> DeletePost(string userId, string postId)
        {
            try
            {
                var response = await _apiService.DeletePost(postId, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }

        }
    }
}

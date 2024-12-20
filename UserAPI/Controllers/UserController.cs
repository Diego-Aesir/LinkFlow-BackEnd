using Microsoft.AspNetCore.Mvc;
using UserAPI.Interfaces;
using UserAPI.Models;
using UserAPI.DTO;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }
        
        
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("None user information was sent");
            }

            await _userService.CreateUserAsync(user);
            return NoContent();
        }
        
        [HttpGet("{user_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUser(string user_id)
        {
            var user = await _userService.GetUserAsync(user_id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPut("update/{user_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateUser(string user_id, [FromBody]  UpdateUser updateUser)
        {
            User user = await _userService.UpdateUserAsync(user_id, updateUser);
            if(user == null)
            {
                return BadRequest("Couldn't update this user: " + updateUser.UserName);
            }

            return Ok(user);
        }

        [HttpDelete("delete/{user_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(string user_id)
        {
            bool deleted = await _userService.DeleteUserAsync(user_id);
            if(!deleted)
            {
                return BadRequest("Couldn't delete this user");
            }

            return Ok($"User deleted: {deleted}");
        }
    }
}

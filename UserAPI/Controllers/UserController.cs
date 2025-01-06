using Microsoft.AspNetCore.Mvc;
using UserAPI.Interfaces;
using UserAPI.Models;
using UserAPI.DTO;
using Microsoft.AspNetCore.Cors;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("SecurityPolicy")]
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
            try
            {
                await _userService.CreateUserAsync(user);
                return NoContent();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }            
        }
        
        [HttpGet("id:{user_id}")]
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

        [HttpGet("username:{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserAsyncByUsername(string username)
        {
            var user = await _userService.GetUserAsyncByUsername(username);
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
            try
            {
                User user = await _userService.UpdateUserAsync(user_id, updateUser);
                return Ok(user);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
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

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            try
            {
                if(string.IsNullOrEmpty(userLogin.Password))
                {
                    return BadRequest("Password is required");
                }

                User user = await _userService.LoginUser(userLogin.UserName, userLogin.Password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to login {ex.Message}");
            }
        }

        [HttpPost("login/google")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginWithGoogle([FromBody] UserLogin userLogin)
        {
            try
            {
                userLogin.Password = null;
                User user = await _userService.LoginUserWithGoogle(userLogin.UserName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to login {ex.Message}");
            }
        }
    }
}

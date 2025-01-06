using LinkFlowWebBFF.DTO;
using LinkFlowWebBFF.Services;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;

namespace LinkFlowWebBFF.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase 
    {
        private readonly ApiService _apiService;

        public UserController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                user.IsGoogle = false;
                var response = await _apiService.CreateUser(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register/google")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserWithGoogle([FromBody] GoogleAuthModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.IdToken))
                {
                    return BadRequest("IdToken is required.");
                }

                var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken);

                User user = new()
                {
                    UserName = $"{payload.GivenName.ToLower()}.{payload.FamilyName.ToLower()}.{payload.Subject.Substring(0, 4)}",
                    Name = payload.Name,
                    UserEmail = payload.Email,
                    Photo = payload.Picture,
                    Gender = "",
                    Pronoun = "",
                    Password = null,
                    Profile = null,
                    IsGoogle = true
                };
                var response = await _apiService.CreateUserWithGoogle(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            try
            {
                if(string.IsNullOrEmpty(user.Password))
                {
                    return BadRequest("Password is required");
                }

                var response = await _apiService.LoginUser(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login/google")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleAuthModel model)
        {
            try
            {
                if(string.IsNullOrEmpty(model.IdToken))
                {
                    return BadRequest("IdToke is required");
                }

                var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken);
                string UserName = $"{payload.GivenName.ToLower()}.{payload.FamilyName.ToLower()}.{payload.Subject.Substring(0, 4)}";

                LoginUser user = new() 
                {
                    UserName = UserName,
                    Password = null
                };
                var response = await _apiService.LoginUserWithGoogle(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var response = await _apiService.GetUserById(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUser user)
        {
            try
            {
                var response = await _apiService.UpdateUser(userId, user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var response = await _apiService.DeleteUser(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
     }
}

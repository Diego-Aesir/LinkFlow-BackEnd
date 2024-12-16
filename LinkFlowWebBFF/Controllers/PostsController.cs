using Microsoft.AspNetCore.Mvc;

namespace LinkFlowWebBFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> GetHomePagePosts()
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var response = await _userService.GetAllAsync(page, size);
            return response != null ? Ok(response) : StatusCode(500);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var response = await _userService.GetAsync(id);
            return response != null ? Ok(response) : StatusCode(500);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var response = await _userService.Create(request);
            return response != null ? Ok(response) : StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateUserRequest request)
        {
            var response = await _userService.Update(id, request);
            return response != null ? Ok(response) : StatusCode(500);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var response = await _userService.Delete(id);
            return response != null ? Ok(response) : StatusCode(500);
        }
    }
}

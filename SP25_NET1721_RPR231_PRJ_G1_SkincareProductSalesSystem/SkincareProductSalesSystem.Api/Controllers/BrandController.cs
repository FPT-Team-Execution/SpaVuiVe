using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("/brands")]
<<<<<<< HEAD
        public async Task<IActionResult> GetAll()
        {
            var responses = await _brandService.GetAll();
=======
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var responses = await _brandService.GetPaginate(page, size);
>>>>>>> develop
            return (responses != null) ? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/brands/{id}")]
<<<<<<< HEAD
        public async Task<IActionResult> GetBrandById([FromHeader] string id)
=======
        public async Task<IActionResult> GetBrandById(string id)
>>>>>>> develop
        {
            var response = await _brandService.GetBrandById(id);
            return (response != null)? Ok(response) : NotFound();
        }
    }
}

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
        public async Task<IActionResult> GetAll()
        {
            var responses = await _brandService.GetAll();
            return (responses != null) ? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/brands/{id}")]
        public async Task<IActionResult> GetBrandById([FromHeader] string id)
        {
            var response = await _brandService.GetBrandById(id);
            return (response != null)? Ok(response) : NotFound();
        }
    }
}

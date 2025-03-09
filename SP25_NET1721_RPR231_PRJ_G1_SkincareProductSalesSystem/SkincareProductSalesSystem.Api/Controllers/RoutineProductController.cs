using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineProductController : ControllerBase
    {
        private readonly IRoutineProductService _routineProductService;

        public RoutineProductController(IRoutineProductService routineProductService)
        {
            _routineProductService = routineProductService;
        }

        [HttpGet("/routine-products")]
        public async Task<IActionResult> GetAllRoutineProducts([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var response = await _routineProductService.GetAllAsync(page: page, size: size);
            return response != null ? Ok(response) : StatusCode(500);
        }

        [HttpGet("/routine-products/{id}")]
        public async Task<IActionResult> GetRoutineProductById([FromRoute] string id)
        {
            var product = await _routineProductService.GetAsync(id);
            return product != null ? Ok(product) : StatusCode(500);
        }

        [HttpPost("/routine-products")]
        public async Task<IActionResult> CreateRoutineProduct([FromBody] RoutineProductRequest request)
        {
            var response = await _routineProductService.Create(request);
            return response != null ? Ok(response) : StatusCode(500);
        }

        [HttpPut("/routine-products/{id}")]
        public async Task<IActionResult> UpdateRoutineProduct([FromRoute] string id, [FromBody] RoutineProductRequest request)
        {
            var response = await _routineProductService.Update(id, request);
            return response != null ? Ok(response) : StatusCode(500);
        }

        [HttpDelete("/routine-products/{id}")]
        public async Task<IActionResult> DeleteRoutineProduct([FromRoute] string id)
        {
            var response = await _routineProductService.Delete(id);
            return response != null ? Ok(response) : StatusCode(500);
        }
    }
}

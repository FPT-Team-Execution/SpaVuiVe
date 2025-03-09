using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkinCareRoutineController : ControllerBase
    {
        private readonly ISkinCareRoutineService _skinCareRoutineService;
        private readonly IRoutineProductService _routineProductService;

        public SkinCareRoutineController(ISkinCareRoutineService skinCareRoutineService, IRoutineProductService routineProductService)
        {
            _skinCareRoutineService = skinCareRoutineService;
            _routineProductService = routineProductService;
        }

        [HttpGet("/skin-care-routines")]
        public async Task<IActionResult> GetAllSkinCareRoutines([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var response = await _skinCareRoutineService.GetAllAsync(page: page, size: size);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [HttpGet("/skin-care-routines/{id}/routine-product")]
        public async Task<IActionResult> GetRoutineProductById([FromRoute] string id)
        {
            var response = await _routineProductService.GetByRoutineId(id);
            return response != null ? Ok(response) : StatusCode(500);
        }

        [HttpGet("/skin-care-routines/{id}")]
        public async Task<IActionResult> GetSkinCareRoutineById([FromRoute] string id)
        {
            var routine = await _skinCareRoutineService.GetAsync(id);
            return routine != null ? Ok(routine) : StatusCode(500);
        }

        [HttpPost("/skin-care-routines")]
        public async Task<IActionResult> CreateSkinCareRoutine([FromForm] SkinCareRoutineRequest request)
        {
            var response = await _skinCareRoutineService.Create(request);
            return response != null ? Ok(response) : StatusCode(500);
        }

        [HttpPut("/skin-care-routines/{id}")]
        public async Task<IActionResult> UpdateSkinCareRoutine([FromRoute] string id, [FromForm] SkinCareRoutineRequest request)
        {
            var response = await _skinCareRoutineService.Update(id, request);
            return response != null ? Ok(response) : StatusCode(500);
        }

        [HttpDelete("/skin-care-routines/{id}")]
        public async Task<IActionResult> DeleteSkinCareRoutine([FromRoute] string id)
        {
            var response = await _skinCareRoutineService.Delete(id);
            return response != null ? Ok(response) : StatusCode(500);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SkinController : ControllerBase
    {
        private readonly ISkinTestService _skinTestService;
        private readonly ISkinTypeService _skinTypeService;

        public SkinController(ISkinTestService skinTestService, ISkinTypeService skinTypeService )
        {
            _skinTestService = skinTestService;
            _skinTypeService = skinTypeService;
        }

        [HttpGet("/skin-tests")]
        public IActionResult GetAllSkinTest()
        {
            var responses = _skinTestService.GetAllAsync();
            return (responses != null) ? Ok(responses.Result) : StatusCode(500);
        }

        [HttpGet("/skin-tests/{id}")]
        public IActionResult GetById(string id)
        {
            var response = _skinTestService.GetAsync(id);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [HttpPost("/skin-tests")]
        public IActionResult CreateSkinTestQuestion(CreateSkinTestRequest request)
        {
            var response = _skinTestService.Create(request);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [HttpPut("/skin-tests/{id}")]
        public IActionResult UpdatekinTestQuestion(string id, UpdateSkinTestRequest request)
        {
            var response = _skinTestService.Update(id, request);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [HttpDelete("/skin-tests/{id}")]
        public IActionResult DeleteSkinTestQuestion(string id)
        {
            var response = _skinTestService.Delete(id);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
    }

}

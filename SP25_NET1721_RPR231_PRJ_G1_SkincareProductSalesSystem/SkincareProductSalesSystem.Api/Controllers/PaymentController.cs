using Azure;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentService;

        public PaymentController(IPaymentServices paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("/payments")]
        public async Task<IActionResult> GetAllPaginate([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var responses = await _paymentService.GetAllPaginate(page, size);
<<<<<<< HEAD
            return (responses != null && responses.Count > 0) ? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/payments/order")]
        public async Task<IActionResult> GetPaymentsByOrderIdPaginate(string orderId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var responses = await _paymentService.GetPaymentsByOrderIdPaginate(orderId,page, size);
            return (responses != null && responses.Count > 0) ? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/payments/{id}")]
        public async Task<IActionResult> GetPaymentById([FromHeader] string id)
=======
            return (responses != null) ? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/payments/order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrderIdPaginate(string orderId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var responses = await _paymentService.GetPaymentsByOrderIdPaginate(orderId,page, size);
            return (responses != null) ? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/payments/{id}")]
        public async Task<IActionResult> GetPaymentById(string id)
>>>>>>> develop
        {
            var response = await _paymentService.GetPaymentById(id);
            return (response != null) ? Ok(response) : NotFound();
        }
    }
}

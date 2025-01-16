using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodServices _paymentMethodServices;

        public PaymentMethodController(IPaymentMethodServices paymentMethodServices)
        {
            _paymentMethodServices = paymentMethodServices;
        }

        [HttpGet("/payment-methods")]
        public async Task<IActionResult> GetAll()
        {
            var responses = await _paymentMethodServices.GetAll();
            return (responses != null && responses.Count > 0)? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/payment-methods/{id}")]
        public async Task<IActionResult> GetPaymentMethodById([FromHeader] string id)
        {
            var response = await _paymentMethodServices.GetPaymentMethodById(id);
            return (response != null)? Ok(response) : NotFound();
        }
    }
}

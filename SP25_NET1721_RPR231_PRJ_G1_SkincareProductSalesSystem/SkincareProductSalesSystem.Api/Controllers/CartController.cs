using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("cart")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var responses = await _cartService.GetUserCartAsync();
            return StatusCode(responses.Status, responses);
        }
        
        [HttpPost("cart")]
        [Authorize]
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {
            var responses = await _cartService.AddOrUpdateToCartAsync(request);
            return StatusCode(responses.Status, responses);
        }
        
        [HttpPatch("cart")]
        [Authorize]
        public async Task<IActionResult> UpdateToCart(UpdateToCartRequest request)
        {
            var responses = await _cartService.UpdateToCartAsync(request);
            return StatusCode(responses.Status, responses);
        }



        [HttpDelete("cart/product/{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(string id)
        {
            var responses = await _cartService.RemoveFromCartAsync(id);
            return StatusCode(responses.Status, responses);
        }
    }
}

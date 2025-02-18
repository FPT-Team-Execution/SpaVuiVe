using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services.ExtendServices;
using SkincareProductSalesSystem.Repositories.Models;
using Azure;
using SkincareProductSalesSystem.Services.Base;
using SkincareProductSalesSystem.Services;
using SkincareProductSalesSystem.Services.Models.Cart;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICacheService _cacheService;

        private String CART_REDIS_KEY = "product_cart";


        public CartController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("cart")]
        public async Task<IActionResult> GetAll(
            //[FromQuery] int page = 1, [FromQuery] int size = 10
            )
        {
            try
            {
                var responses = await _cacheService.GetDataAsync<List<ProductCart>>(CART_REDIS_KEY);
                return Ok(new ServiceResult
                {
                    Status = 200,
                    Message = (responses != null && responses.Count > 0) ? "" : "There is not any product in cart",
                    Data = (responses != null && responses.Count > 0) ? responses : null
                });
            }
            catch (Exception e)
            {
                return Ok(new ServiceResult
                {
                    Status = 500,
                    Message = e.Message
                });
            }
        }

        [HttpPost("cart/product")]
        public async Task<IActionResult> AddToCart(Product product, int quantity)
        {
            try
            {
                var oldCart = await _cacheService.GetDataAsync<List<ProductCart>>(CART_REDIS_KEY);
                if (oldCart != null) 
                {
                    var existingProductCart = oldCart.FirstOrDefault(item => item.ProductInCart.ProductId == product.ProductId);
                    if (existingProductCart == null)
                    {
                        oldCart.Add(new ProductCart { ProductInCart = product, Quantity = quantity });
                    }
                    else
                    {
                        oldCart.Remove(existingProductCart);
                        existingProductCart.Quantity += quantity;
                        oldCart.Add(existingProductCart);
                    }
                    var response = await _cacheService.SetDataAsync(CART_REDIS_KEY, oldCart);
                    return Ok(new ServiceResult
                    {
                        Status = 200,
                        Message = (response) ? "Success" : "Fail ",
                    });
                }
                else
                {
                    var newCart = new List<ProductCart>();
                    newCart.Add(new ProductCart{ProductInCart = product, Quantity = quantity});
                    var response = await _cacheService.SetDataAsync(CART_REDIS_KEY, newCart);
                    return Ok(new ServiceResult
                    {
                        Status = 200,
                        Message = (response)? "Success" : "Fail ",
                    });
                }
                
            }
            catch (Exception e)
            {
                return Ok(new ServiceResult
                {
                    Status = 500,
                    Message = e.Message
                });
            }
        }
    }
}

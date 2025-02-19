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

        [HttpGet("cart/product/{productId}")]
        public async Task<IActionResult> GetProductInCartById(string productId)
        {
            var oldCart = await _cacheService.GetDataAsync<List<ProductCart>>(CART_REDIS_KEY);
            if (oldCart != null)
            {
                var existingProductCart = oldCart.FirstOrDefault(item =>
    item.ProductInCart.ProductId != null && item.ProductInCart.ProductId.Equals(productId));

                return Ok(existingProductCart);
            }
            return Ok(null);
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
                    newCart.Add(new ProductCart { ProductInCart = product, Quantity = quantity });
                    var response = await _cacheService.SetDataAsync(CART_REDIS_KEY, newCart);
                    return Ok(new ServiceResult
                    {
                        Status = 200,
                        Message = (response) ? "Success" : "Fail ",
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
        [HttpPut("cart/product")]
        public async Task<IActionResult> UpdateToCart(Product product, int quantity)
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
                        existingProductCart.Quantity = quantity;
                        oldCart.Add(existingProductCart);
                    }
                    var response = await _cacheService.SetDataAsync(CART_REDIS_KEY, oldCart);
                    return Ok(new ServiceResult
                    {
                        Status = (response) ? 200 : 500,
                        Message = (response) ? "Success" : "Fail ",
                    });
                }
                else
                {
                    var newCart = new List<ProductCart>();
                    newCart.Add(new ProductCart { ProductInCart = product, Quantity = quantity });
                    var response = await _cacheService.SetDataAsync(CART_REDIS_KEY, newCart);
                    return Ok(new ServiceResult
                    {
                        Status = (response) ? 200 : 500,
                        Message = (response) ? "Success" : "Fail ",
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

        [HttpDelete("cart/product/{id}")]
        public async Task<IActionResult> RemoveFromCart(string id)
        {
            try
            {
                var oldCart = await _cacheService.GetDataAsync<List<ProductCart>>(CART_REDIS_KEY);
                if (oldCart != null)
                {
                    var existingProductCart = oldCart.FirstOrDefault(item => item.ProductInCart.ProductId == id);
                    if (existingProductCart == null) return NotFound(new ServiceResult { Status = 404, Message = "Product is not found" });
                    oldCart.Remove(existingProductCart);
                    var response = await _cacheService.SetDataAsync(CART_REDIS_KEY, oldCart);
                    return Ok(new ServiceResult
                    {
                        Status = (response) ? 200 : 500,
                        Message = (response) ? "Success" : "Fail ",
                    });
                }
                return Ok(new ServiceResult
                {
                    Status = 400,
                    Message = "Fail"
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
    }
}

using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailServices _orderDetailServices;

        public OrderDetailController(IOrderDetailServices orderDetailServices)
        {
            _orderDetailServices = orderDetailServices;
        }

        [HttpGet("/order-details/{orderId}")]
<<<<<<< HEAD
        public async Task<IActionResult> GetOrderDetailsByOrderId([FromHeader]  string orderId)
        {
            var responses = await _orderDetailServices.GetOrderDetailsByOrderId(orderId);
            return (responses.Count > 0)? Ok(responses) : StatusCode(500);
=======
        public async Task<IActionResult> GetOrderDetailsByOrderId(string orderId, [FromQuery] int page, [FromQuery] int size)
        {
            var responses = await _orderDetailServices.GetOrderDetailsByOrderId(
                    orderId: orderId,
                    page: page,
                    size: size
                );
            return (responses != null)? Ok(responses) : StatusCode(500);
>>>>>>> develop
        }

        [HttpPost("/order-details")]
        public async Task<IActionResult> CreateOrderDetail(OrderDetail orderDetail)
        {
            var response = await _orderDetailServices.CreateOrderDetail(orderDetail);
            return (response != null)? Ok(response) : StatusCode(300);
        }

        [HttpPatch("/order-details")]
        public async Task<IActionResult> UpdateOrderDetail(OrderDetail orderDetail)
        {
            var response = await _orderDetailServices.UpdateOrderDetail(orderDetail);
            return (response != null) ? Ok(response) : StatusCode(300);
        }
    }
}

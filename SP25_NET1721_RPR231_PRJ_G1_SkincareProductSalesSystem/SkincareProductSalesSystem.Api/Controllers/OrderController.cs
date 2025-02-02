<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
>>>>>>> develop
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet("/orders")]
<<<<<<< HEAD
        public async Task<IActionResult> GetAll([FromQuery]int page = 1, [FromQuery] int size = 10) 
        {
            var responses = await _orderServices.GetPagination(page, size);
            return (responses.Count > 0)? Ok(responses) : StatusCode(500); 
        }

        [HttpGet("/orders/{id}")]
        public async Task<IActionResult> GetOrderById([FromHeader] string id)
        {
            var response = await _orderServices.GetOrderById(id);
            return (response != null)? Ok(response) : NotFound();
=======
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var responses = await _orderServices.GetPagination(page, size);
            return (responses != null) ? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/orders/{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var response = await _orderServices.GetOrderById(id);
            return (response != null) ? Ok(response) : NotFound();
>>>>>>> develop
        }

        [HttpPost("/orders")]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var response = await _orderServices.CreateOrder(order);
<<<<<<< HEAD
            return (response != null)? Ok(response) : StatusCode(300); 
=======
            return (response != null) ? Ok(response) : StatusCode(300);
>>>>>>> develop
        }

        [HttpPatch("/orders")]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            var response = await _orderServices.UpdateOrder(order);
<<<<<<< HEAD
            return (response != null)? Ok(response) : StatusCode(300);
=======
            return (response != null) ? Ok(response) : StatusCode(300);
>>>>>>> develop
        }
    }
}

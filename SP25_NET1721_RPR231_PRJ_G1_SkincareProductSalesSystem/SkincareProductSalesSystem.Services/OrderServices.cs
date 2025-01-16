using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface IOrderServices
    {
        Task<List<Order>> GetPagination(int page, int size);
        Task<Order?> GetOrderById(string id);
        Task<Order?> CreateOrder(Order order);
        Task<Order?> UpdateOrder(Order order);
    }
    public class OrderServices : IOrderServices
    {
        private readonly OrderRepository _orderRepository;

        public OrderServices(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> CreateOrder(Order order)
        {
            return await _orderRepository.CreateOrder(order);
        }

        public async Task<Order?> GetOrderById(string id)
        {
            return await _orderRepository.GetOrderById(id);
        }

        public async Task<List<Order>> GetPagination(int page, int size)
        {
            return await _orderRepository.GetOrderPagination(page, size);
        }

        public async Task<Order?> UpdateOrder(Order order)
        {
            return await _orderRepository.UpdateOrder(order);
        }
    }
}

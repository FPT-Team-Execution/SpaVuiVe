using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
<<<<<<< HEAD
=======
using SkincareProductSalesSystem.Repositories.Paginate;
>>>>>>> develop
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface IOrderServices
    {
<<<<<<< HEAD
        Task<List<Order>> GetPagination(int page, int size);
=======
        Task<IPaginate<Order>> GetPagination(int page, int size);
>>>>>>> develop
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
<<<<<<< HEAD
            return await _orderRepository.CreateOrder(order);
=======
            return ((await _orderRepository.CreateAsync(order)) > 0)? order : null;
>>>>>>> develop
        }

        public async Task<Order?> GetOrderById(string id)
        {
<<<<<<< HEAD
            return await _orderRepository.GetOrderById(id);
        }

        public async Task<List<Order>> GetPagination(int page, int size)
        {
            return await _orderRepository.GetOrderPagination(page, size);
        }

        public async Task<Order?> UpdateOrder(Order order)
        {
            return await _orderRepository.UpdateOrder(order);
=======
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IPaginate<Order>> GetPagination(int page, int size)
        {
            return await _orderRepository.GetPagingListAsync(
                page:page,
                size:size
                );
        }
        public async Task<Order?> UpdateOrder(Order order)
        {
            return ((await _orderRepository.UpdateAsync(order)) > 0)? order : null;
>>>>>>> develop
        }
    }
}

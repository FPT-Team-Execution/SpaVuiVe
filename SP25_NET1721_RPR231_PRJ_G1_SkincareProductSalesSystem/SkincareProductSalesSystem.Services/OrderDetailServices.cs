using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface IOrderDetailServices
    {
        Task<List<OrderDetail>> GetOrderDetailsByOrderId(string orderId);
        Task<OrderDetail?> CreateOrderDetail(OrderDetail newOrderDetail);
        Task<OrderDetail?> UpdateOrderDetail(OrderDetail updateOrderDetail);
    }
    public class OrderDetailServices : IOrderDetailServices
    {
        private readonly OrderDetailRepository _orderDetailRepository;

        public OrderDetailServices(OrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<OrderDetail?> CreateOrderDetail(OrderDetail newOrderDetail)
        {
            return await _orderDetailRepository.CreateOrderDetail(newOrderDetail);
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderId(string orderId)
        {
            return await _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
        }

        public async Task<OrderDetail?> UpdateOrderDetail(OrderDetail updateOrderDetail)
        {
            return await _orderDetailRepository.UpdateOrderDetail(updateOrderDetail);
        }
    }
}

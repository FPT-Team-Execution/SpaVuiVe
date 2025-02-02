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
    public interface IOrderDetailServices
    {
<<<<<<< HEAD
        Task<List<OrderDetail>> GetOrderDetailsByOrderId(string orderId);
=======
        Task<IPaginate<OrderDetail>> GetOrderDetailsByOrderId(int page, int size, string orderId);
>>>>>>> develop
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
<<<<<<< HEAD
            return await _orderDetailRepository.CreateOrderDetail(newOrderDetail);
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderId(string orderId)
        {
            return await _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
=======
            return ((await _orderDetailRepository.CreateAsync(newOrderDetail)) > 0)? newOrderDetail : null;
        }

        public async Task<IPaginate<OrderDetail>> GetOrderDetailsByOrderId(int page, int size, string orderId)
        {
            return await _orderDetailRepository.GetPagingListAsync(
                    predicate: x => x.OrderId == orderId,
                    size: size,
                    page: page
                );
>>>>>>> develop
        }

        public async Task<OrderDetail?> UpdateOrderDetail(OrderDetail updateOrderDetail)
        {
<<<<<<< HEAD
            return await _orderDetailRepository.UpdateOrderDetail(updateOrderDetail);
=======
            return ((await _orderDetailRepository.UpdateAsync(updateOrderDetail)) > 0)? updateOrderDetail : null;
>>>>>>> develop
        }
    }
}

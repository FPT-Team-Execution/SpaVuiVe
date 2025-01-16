using KoiMuseum.Data.Base;
using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>
    {
        public OrderDetailRepository() { }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderId(string orderId)
        {
            return await _context.OrderDetails.Where(x => x.OrderId.Equals(orderId)).ToListAsync();
        }

        public async Task<OrderDetail?> CreateOrderDetail(OrderDetail newOrderDetail)
        {
            var order = await _context.Orders.FindAsync(newOrderDetail.OrderId);
            if (order == null) { return null; }
            var orderDetail = await _context.OrderDetails.FindAsync(newOrderDetail.OrderDetailId);
            if (orderDetail != null) { return null; }
            await _context.OrderDetails.AddAsync(newOrderDetail);
            var isSuccessfull = (await _context.SaveChangesAsync()) > 0;
            return isSuccessfull? orderDetail : null;
        }

        public async Task<OrderDetail?> UpdateOrderDetail(OrderDetail updateOrderDetail)
        {
            var order = await _context.Orders.FindAsync(updateOrderDetail.OrderId);
            if (order == null) { return null; }
            var orderDetail = await _context.OrderDetails.FindAsync(updateOrderDetail.OrderDetailId);
            if (orderDetail == null) { return null; }
            _context.OrderDetails.Update(updateOrderDetail);
            var isSuccessfull = (await _context.SaveChangesAsync()) > 0;
            return isSuccessfull ? orderDetail : null;
        }
    }
}
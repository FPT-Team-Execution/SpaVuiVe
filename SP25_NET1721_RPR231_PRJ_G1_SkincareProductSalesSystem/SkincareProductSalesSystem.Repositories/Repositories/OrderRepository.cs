using KoiMuseum.Data.Base;
using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository() { }
        public async Task<List<Order>> GetOrderPagination(int page, int size)
        {
            return await _context.Orders.Skip((page - 1) * size).Take(size).ToListAsync();
        }

        public async Task<Order?> GetOrderById(string id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order?> CreateOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            var isSuccessful = (await _context.SaveChangesAsync()) > 0;
            return isSuccessful ? order : null;
        }

        public async Task<Order?> UpdateOrder(Order order)
        {
            var oldOrder = await _context.Orders.FindAsync(order.OrderId);
            if (oldOrder == null) { return null; }
            _context.UpdateRange(order);
            var isSuccessful = await (_context.SaveChangesAsync()) > 0;
            return isSuccessful ? order : null;
        }
    }
}
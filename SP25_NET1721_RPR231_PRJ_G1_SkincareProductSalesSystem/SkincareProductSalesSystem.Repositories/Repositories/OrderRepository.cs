using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;
using System.Collections;

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
        public async Task<IEnumerable<Order>> Search(DateTime start, DateTime end, string status, string userId)
        {
            return await _context.Orders
                        .Where(x => (start == null || x.OrderDate >= start) 
                        &&(end == null || x.OrderDate <= end) 
                        && (string.IsNullOrEmpty(status) || x.Status.Equals(status))
                        &&(x.Customer.UserId.Equals(userId)) || String.IsNullOrEmpty(userId))
                        .ToListAsync();
        }
    }
}
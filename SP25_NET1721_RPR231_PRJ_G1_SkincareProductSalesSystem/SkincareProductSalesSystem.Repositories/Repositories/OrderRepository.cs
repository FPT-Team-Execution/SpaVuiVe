using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;
using System.Collections;

namespace SkincareProductSalesSystem.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
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
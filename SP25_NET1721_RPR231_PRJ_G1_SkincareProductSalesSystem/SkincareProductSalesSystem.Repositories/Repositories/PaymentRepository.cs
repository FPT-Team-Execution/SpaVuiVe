using Microsoft.EntityFrameworkCore;
﻿using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository() { }
        public async Task<List<Payment>> GetAllPaginate(int page, int size)
        {
            return await _context.Payments.Skip((page - 1)  * size).Take(size).ToListAsync();
        }

        public async Task<Payment?> GetPaymentById(string id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<List<Payment>> GetPaymentsByOrderIdPaginate(string orderId, int page, int size)
        {
            return await _context.Payments.Where(x => x.OrderId.Equals(x.OrderId)).Skip((page - 1) * size).Take(size).ToListAsync();
        }

    }
}
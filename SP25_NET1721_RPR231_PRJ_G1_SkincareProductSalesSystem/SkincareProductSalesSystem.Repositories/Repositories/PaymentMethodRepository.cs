using Microsoft.EntityFrameworkCore;
﻿using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod>
    {
        public PaymentMethodRepository() { }
        public async Task<List<PaymentMethod>> GetAll()
        {
            return await _context.PaymentMethods.ToListAsync();
        }

        public async Task<PaymentMethod?> GetPaymentMethodById(string id)
        {
            return await _context.PaymentMethods.FindAsync(id);
        }
    }
}
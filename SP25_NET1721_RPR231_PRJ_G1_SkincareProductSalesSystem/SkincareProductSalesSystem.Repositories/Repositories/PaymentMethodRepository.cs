<<<<<<< HEAD
﻿using KoiMuseum.Data.Base;
using Microsoft.EntityFrameworkCore;
=======
﻿using SkincareProductSalesSystem.Repositories.Base;
>>>>>>> develop
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod>
    {
        public PaymentMethodRepository() { }
<<<<<<< HEAD
        //public async Task<List<PaymentMethod>> GetAll()
        //{
        //    return await _context.PaymentMethods.ToListAsync();
        //}

        //public async Task<PaymentMethod?> GetPaymentMethodById(string id)
        //{
        //    return await _context.PaymentMethods.FindAsync(id);
        //}
=======
>>>>>>> develop
    }
}
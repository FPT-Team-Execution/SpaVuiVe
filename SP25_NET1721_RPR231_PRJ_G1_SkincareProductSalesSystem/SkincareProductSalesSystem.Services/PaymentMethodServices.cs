using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface IPaymentMethodServices
    {
        Task<IPaginate<PaymentMethod>> GetAll(int page, int size);
        Task<PaymentMethod?> GetPaymentMethodById(string id);
    }
    public class PaymentMethodServices : IPaymentMethodServices
    {
        private readonly PaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodServices(PaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<IPaginate<PaymentMethod>> GetAll(int page, int size)
        {
            return await _paymentMethodRepository.GetPagingListAsync(
                    size: size,
                    page: page
                );
        }

        public async Task<PaymentMethod?> GetPaymentMethodById(string id)
        {
            return await _paymentMethodRepository.GetByIdAsync(id);
        }

        public async Task<PaymentMethod?> UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            return ((await _paymentMethodRepository.UpdateAsync(paymentMethod)) > 0)? paymentMethod : null;
        }
    }
}

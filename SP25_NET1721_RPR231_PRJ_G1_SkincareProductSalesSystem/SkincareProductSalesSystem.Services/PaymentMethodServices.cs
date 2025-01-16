using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface IPaymentMethodServices
    {
        Task<List<PaymentMethod>> GetAll();
        Task<PaymentMethod?> GetPaymentMethodById(string id);
    }
    public class PaymentMethodServices : IPaymentMethodServices
    {
        private readonly PaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodServices(PaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<List<PaymentMethod>> GetAll()
        {
            return await _paymentMethodRepository.GetAllAsync();
        }

        public async Task<PaymentMethod?> GetPaymentMethodById(string id)
        {
            return await _paymentMethodRepository.GetByIdAsync(id);
        }
    }
}

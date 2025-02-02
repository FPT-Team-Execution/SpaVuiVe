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
    public interface IPaymentMethodServices
    {
<<<<<<< HEAD
        Task<List<PaymentMethod>> GetAll();
=======
        Task<IPaginate<PaymentMethod>> GetAll(int page, int size);
>>>>>>> develop
        Task<PaymentMethod?> GetPaymentMethodById(string id);
    }
    public class PaymentMethodServices : IPaymentMethodServices
    {
        private readonly PaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodServices(PaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

<<<<<<< HEAD
        public async Task<List<PaymentMethod>> GetAll()
        {
            return await _paymentMethodRepository.GetAllAsync();
=======
        public async Task<IPaginate<PaymentMethod>> GetAll(int page, int size)
        {
            return await _paymentMethodRepository.GetPagingListAsync(
                    size: size,
                    page: page
                );
>>>>>>> develop
        }

        public async Task<PaymentMethod?> GetPaymentMethodById(string id)
        {
            return await _paymentMethodRepository.GetByIdAsync(id);
        }
<<<<<<< HEAD
=======

        public async Task<PaymentMethod?> UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            return ((await _paymentMethodRepository.UpdateAsync(paymentMethod)) > 0)? paymentMethod : null;
        }
>>>>>>> develop
    }
}

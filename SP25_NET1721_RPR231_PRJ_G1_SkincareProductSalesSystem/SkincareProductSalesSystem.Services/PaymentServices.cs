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
    public interface IPaymentServices
    {
<<<<<<< HEAD
        Task<List<Payment>> GetAllPaginate(int page, int size);
        Task<Payment?> GetPaymentById(string id);
        Task<List<Payment>> GetPaymentsByOrderIdPaginate(string orderId, int page, int size);
=======
        Task<IPaginate<Payment>> GetAllPaginate(int page, int size);
        Task<Payment?> GetPaymentById(string id);
        Task<IPaginate<Payment>> GetPaymentsByOrderIdPaginate(string orderId, int page, int size);
>>>>>>> develop
    }
    public class PaymentServices : IPaymentServices
    {
        private readonly PaymentRepository _paymentRepository;

        public PaymentServices(PaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

<<<<<<< HEAD
        public async Task<List<Payment>> GetAllPaginate(int page, int size)
        {
            return await _paymentRepository.GetAllPaginate(page, size);
=======
        public async Task<IPaginate<Payment>> GetAllPaginate(int page, int size)
        {
            return await _paymentRepository.GetPagingListAsync
                ( 
                    page: page,
                    size: size
                );
>>>>>>> develop
        }

        public async Task<Payment?> GetPaymentById(string id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }

<<<<<<< HEAD
        public async Task<List<Payment>> GetPaymentsByOrderIdPaginate(string orderId, int page, int size)
        {
            return await _paymentRepository.GetPaymentsByOrderIdPaginate(orderId, page, size);
=======
        public async Task<IPaginate<Payment>> GetPaymentsByOrderIdPaginate(string orderId, int page, int size)
        {
            return await _paymentRepository.GetPagingListAsync(
                predicate: x => x.OrderId.Equals(orderId),
                page: page,
                size: size
                );
>>>>>>> develop
        }
    }
}

using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface IPaymentServices
    {
        Task<List<Payment>> GetAllPaginate(int page, int size);
        Task<Payment?> GetPaymentById(string id);
        Task<List<Payment>> GetPaymentsByOrderIdPaginate(string orderId, int page, int size);
    }
    public class PaymentServices : IPaymentServices
    {
        private readonly PaymentRepository _paymentRepository;

        public PaymentServices(PaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<List<Payment>> GetAllPaginate(int page, int size)
        {
            return await _paymentRepository.GetAllPaginate(page, size);
        }

        public async Task<Payment?> GetPaymentById(string id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }

        public async Task<List<Payment>> GetPaymentsByOrderIdPaginate(string orderId, int page, int size)
        {
            return await _paymentRepository.GetPaymentsByOrderIdPaginate(orderId, page, size);
        }
    }
}

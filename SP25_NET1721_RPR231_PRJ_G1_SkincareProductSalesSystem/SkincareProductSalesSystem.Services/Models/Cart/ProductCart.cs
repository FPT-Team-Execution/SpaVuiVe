using SkincareProductSalesSystem.Repositories.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.Cart
{
    public class ProductCart
    {
        [Required]
        public Product ProductInCart {  get; set; }

        [Required]
        public int Quantity {  get; set; }
    }
}

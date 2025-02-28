using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface ICartService
    {
        bool IsItemExistInList<T>(string id, List<T> list) where T : class;
    }
    public class CartService
    {
        public CartService() { }
        public bool IsItemExistInList<T>(string id, List<T> list) where T : class
        {
            if (list == null || list.Count == 0) return false;

            var property = typeof(T).GetProperty("id");
            if (property == null) throw new ArgumentException("T must have an 'id' property");

            foreach (var item in list)
            {
                var itemId = property.GetValue(item)?.ToString();
                if (itemId != null && itemId.Equals(id)) return true;
            }

            return false;
        }

    }
}

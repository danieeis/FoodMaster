using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodMaster.Models;

namespace FoodMaster.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersByUser(string userEmail);
        Task StoreOrder(Order order);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DutchTreatCore.Data.Entities;

namespace DutchTreatCore.Repositories
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
        void AddOrder(Order order);
        IEnumerable<Order> GetAllOrdersByUser(string userName);
    }
}

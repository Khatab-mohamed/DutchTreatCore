using System.Collections.Generic;
using System.Linq;
using DutchTreatCore.Data;
using DutchTreatCore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DutchTreatCore.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly DutchContext _context;

        public OrdersRepository(DutchContext context)
        {
            _context = context;
        }
        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.Include(o=>o.Items).ToList();
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders.Include(o => o.Items).FirstOrDefault(o=> o.Id ==id);
        }

        public void AddOrder(Order order)
        {
            _context.Add(order);
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetAllOrdersByUser(string userName)
        {
            return _context.Orders
                .Include(o => o.Items)
                .Where(o=>o.User.UserName == userName)
                .ToList();
            ;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreatCore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreatCore.Controllers
{
    [Route("/api/orders/{orderId}/items")]
    public class OrdersItemsController: Controller
    {
        private readonly IOrdersRepository _repository;

        public OrdersItemsController(IOrdersRepository repository )
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            var order = _repository.GetOrderById(orderId);
            if (order != null) return Ok(order.Items);
            return NotFound();
        }

        [HttpGet("{orderItemId}")]
        public IActionResult Get(int orderId, int orderItemId)
        {
            var order = _repository.GetOrderById(orderId);
            var item = order?.Items.FirstOrDefault(i => i.Id == orderItemId);
            if (item != null)
                return Ok(item);

            return NotFound();
        }
        
    }
}

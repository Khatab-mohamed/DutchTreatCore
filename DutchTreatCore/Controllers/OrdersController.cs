using System;
using DutchTreatCore.Data.Entities;
using DutchTreatCore.Repositories;
using DutchTreatCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreatCore.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _repository;

        public OrdersController(IOrdersRepository ordersRepository)
        {
            _repository = ordersRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _repository.GetAllOrders();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var order = _repository.GetOrderById(id);
            if (order == null)
                return NotFound();
            else
                return Ok(order);
        }

        [HttpPost]
        public IActionResult Post(OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                var newOrder = new Order
                {
                    OrderDate = order.OrderDate,
                    OrderNumber = order.OrderNumber,
                    Id = order.OrderId
                };

                if (newOrder.OrderDate==DateTime.MinValue)
                    newOrder.OrderDate = DateTime.Now;

                _repository.AddOrder(newOrder );
               
                var orderViewModel = new OrderViewModel
                {
                    OrderId = newOrder.Id,
                    OrderDate = newOrder.OrderDate,
                    OrderNumber = newOrder.OrderNumber
                };

                return Created($"/api/orders/{orderViewModel.OrderId}", orderViewModel);
            }

            return null;
        }
    }
}
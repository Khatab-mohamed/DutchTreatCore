using System;
using System.Collections.Generic;
using AutoMapper;
using DutchTreatCore.Data.Entities;
using DutchTreatCore.Repositories;
using DutchTreatCore.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreatCore.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _repository;
        private readonly IMapper _mapper;

        public OrdersController(IOrdersRepository ordersRepository,IMapper mapper)
        {
            _repository = ordersRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_mapper.Map<IEnumerable<Order>,IEnumerable<OrderViewModel>>(_repository.GetAllOrders()));
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var order = _repository.GetOrderById(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                var newOrder = _mapper.Map< OrderViewModel, Order>(order);

                if (newOrder.OrderDate==DateTime.MinValue)
                    newOrder.OrderDate = DateTime.Now;

                _repository.AddOrder(newOrder );
               
            
                return Created($"/api/orders/{newOrder.Id}", _mapper.Map< Order, OrderViewModel>(newOrder));
            }

            return null;
        }
    }
}
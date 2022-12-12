using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreatCore.Data.Entities;
using DutchTreatCore.Repositories;
using DutchTreatCore.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreatCore.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IOrdersRepository ordersRepository,
            IMapper mapper,
            UserManager<StoreUser> userManager)
        {
            _repository = ordersRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

    
        [HttpGet]
        public IActionResult Get()
        {
            var userName = User.Identity.Name;

            return Ok(_mapper.Map<IEnumerable<Order>,IEnumerable<OrderViewModel>>(_repository.GetAllOrdersByUser(userName)));
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
        public async Task<IActionResult> Post([FromBody] OrderViewModel order)
        {
            if (!ModelState.IsValid) return null;
            var newOrder = _mapper.Map< OrderViewModel, Order>(order);

            if (newOrder.OrderDate==DateTime.MinValue)
                newOrder.OrderDate = DateTime.Now;
            //Add the user  ordering the order
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            //Assign the Current   order to the user
            newOrder.User = currentUser;

            _repository.AddOrder(newOrder );
               
            
            return Created($"/api/orders/{newOrder.Id}", _mapper.Map< Order, OrderViewModel>(newOrder));

        }
    }
}
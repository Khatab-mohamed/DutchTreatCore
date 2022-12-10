using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreatCore.Data.Entities;
using DutchTreatCore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreatCore.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductsRepository _productsRepository;

        public ProductController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products=  _productsRepository.GetAllProducts();
            return Ok(products);
        } 

    }
}

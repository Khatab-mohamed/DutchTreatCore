using System;
using System.Linq;
using System.Net;
using DutchTreatCore.Data;
using DutchTreatCore.Repositories;
using DutchTreatCore.Services;
using DutchTreatCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DutchTreatCore.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IProductsRepository _repository;


        public AppController(IMailService mailService, IProductsRepository repository)
        {
            _mailService = mailService;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid) return View();
            _mailService.SendMessage("khatapit@gmail.com", model.Subject,
                $"From {model.Name} - {model.Email}, Message:{model.Message}");
            ModelState.Clear();
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }

        [Authorize]
        public IActionResult Shop()
        {
            var results = _repository.GetAllProducts();
            return View(results);
        }
    }
}
using System;
using System.Linq;
using DutchTreatCore.Data;
using DutchTreatCore.Services;
using DutchTreatCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DutchTreatCore.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly DutchContext _context;


        public AppController(IMailService mailService, DutchContext context)
        {
            _mailService = mailService;
            _context = context;
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
            _mailService.SendMessage("khatapit@gmail.com",model.Subject,$"From {model.Name} - {model.Email}, Message:{model.Message}");
            ModelState.Clear();
            return View();
        }
        public IActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }

        public IActionResult Shop()
        {
            var results = _context.Products.ToList().OrderBy(p => p.Category);
            return View(results);
        }
    }
}
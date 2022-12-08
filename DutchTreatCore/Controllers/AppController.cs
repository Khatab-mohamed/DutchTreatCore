using System;
using DutchTreatCore.Services;
using DutchTreatCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreatCore.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;

        public AppController(IMailService mailService)
        {
            _mailService = mailService;
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
//ViewBag.UserMessage("Mail Sent");
            ModelState.Clear();
            return View();
        }
        public IActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }
    }
}
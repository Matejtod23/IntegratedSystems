﻿using Microsoft.AspNetCore.Mvc;

namespace EShopAdminApplication.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

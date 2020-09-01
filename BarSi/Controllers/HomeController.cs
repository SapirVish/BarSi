using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarSi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            IsAdmin();
            return View();
        }

        private bool IsAdmin()
        {
            bool isAdmin = (HttpContext != null) && (HttpContext.Session != null) &&
                                 (HttpContext.Session.GetString("IsAdmin") == "true");
            ViewData["IsAdmin"] = isAdmin;
            return isAdmin;
        }
    }
}

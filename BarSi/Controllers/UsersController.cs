﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarSi.Data;
using BarSi.Models;
using Microsoft.AspNetCore.Http;

namespace BarSi.Controllers
{
    public class UsersController : Controller
    {
        private readonly BarSiContext _context;

        public UsersController(BarSiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.Name == username);

            if (user != null && user.Password == password)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout(string username, string password)
        {
            HttpContext.Session.SetString("IsAdmin", "false");
            return RedirectToAction("Index", "Home");
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
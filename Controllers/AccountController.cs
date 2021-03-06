﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoListWebApp.Context;
using ToDoListWebApp.Models;
using ToDoListWebApp.ViewModels.Account;

namespace ToDoListWebApp.Controllers
{
	[Controller]
	public class AccountController : Controller
	{
		private readonly ToDoListContext _context;
        private readonly ILogger<AccountController> _logger;

		public AccountController(ToDoListContext context,
            ILogger<AccountController> logger)
		{
			_context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _logger.LogInformation("Login user.");
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user.Email, user.Id); // аутентификация

                    return RedirectToAction("Index", "Index");
                }
                _logger.LogWarning("Incorrect login or password.");
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            _logger.LogInformation("Register user.");
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    await _context.Users.AddAsync(new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateOfBirth = model.DateOfBirth,
                        Email = model.Email, 
                        Password = model.Password
                    });
                    await _context.SaveChangesAsync();
                    
                    user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                    await Authenticate(user.Email, user.Id);

                    return RedirectToAction("Index", "Index");
                }
                else
                {
                    _logger.LogWarning("Incorrect login or password.");
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(model);
        }

        private async Task Authenticate(string userName, Guid userid)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            Response.Cookies.Append("userid", userid.ToString());
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("userid");
            return RedirectToAction("Login", "Account");
        }
    }
}

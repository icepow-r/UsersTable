using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UsersTable.Data;
using UsersTable.Models;

namespace UsersTable.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UsersTableContext _appContext;
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;

        public HomeController(UsersTableContext appContext, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _appContext = appContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            var users = from s in _appContext.Users select s;
            return View(await users.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> Delete(IFormCollection collection)
        {
            var ids = collection["idcheck"];
            var entities = new List<User>();
            foreach (var item in ids)
            {
                entities.Add(await _appContext.Users.FindAsync(item));
            }
            foreach (var item in ids)
            {
                var user = await _appContext.Users.FindAsync(item);
                if (User.Identity.Name == user.Email)
                {
                    await _signInManager.SignOutAsync();
                }
                else
                {
                    var u = await _userManager.FindByIdAsync(user.Id);
                    await _userManager.UpdateSecurityStampAsync(u);
                }
                _appContext.Remove(user);
            }
            await _appContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Block(IFormCollection collection)
        {
            var ids = collection["idcheck"];
            
            foreach (var item in ids)
            {
                var user = await _appContext.Users.FindAsync(item);
                user.Status = Status.Blocked;
                user.LockoutEnabled = true;
                if (User.Identity.Name == user.Email)
                {
                    await _signInManager.SignOutAsync();
                }
                else
                {
                    var u = await _userManager.FindByIdAsync(user.Id);
                    await _userManager.UpdateSecurityStampAsync(u);
                }
                _appContext.Update(user);
            }
            await _appContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(IFormCollection collection)
        {
            string[] ids = collection["idcheck"];
            foreach (var item in ids)
            {
                var user = await _appContext.Users.FindAsync(item);
                user.Status = Status.Active;
                user.LockoutEnabled = false;
                _appContext.Update(user);
            }
            await _appContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

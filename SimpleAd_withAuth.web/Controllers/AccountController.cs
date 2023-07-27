using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAd_withAuth.data;
using System.Security.Claims;

namespace SimpleAd_withAuth.web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAds;Integrated Security=true;";

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(Lister lister, string password)
        {
            var manager = new DbManager(_connectionString);
            manager.NewLister(lister, password);

            return RedirectToAction("login");
        }

        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var manager = new DbManager(_connectionString);

            var lister = manager.Login(email, password);
            if (lister == null)
            {
                TempData["message"] = "*Invalid Login!";
                return RedirectToAction("login");
            }

            var claims = new List<Claim>
            {

                new Claim("lister", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "lister", "role"))).Wait();



            return Redirect("/home/newad");
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("login");
        }
    }
}

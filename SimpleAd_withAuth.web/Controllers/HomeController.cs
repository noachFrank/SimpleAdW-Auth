using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAd_withAuth.data;
using SimpleAd_withAuth.web.Models;
using System.Diagnostics;

namespace SimpleAd_withAuth.web.Controllers
{

    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAds;Integrated Security=true;";

        public IActionResult Index()
        {
            var manager = new DbManager(_connectionString);

            var vm = new AdViewModel
            {
                AllAds = manager.GetAllAds()
            };
            if (User.Identity.IsAuthenticated)
            {
                var lister = manager.GetByEmail(User.Identity.Name);

                foreach (Ad ad in vm.AllAds)
                {
                    ad.Delete = ad.ListerId == lister.Id;
                }
            }

            return View(vm);
        }

        public IActionResult NewAd()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/account/login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult NewAd(Ad ad)
        {
            var manager = new DbManager(_connectionString);
            var lister = manager.GetByEmail(User.Identity.Name);
            ad.Name = lister.Name;
            ad.ListingDate = DateTime.Today;
            ad.ListerId = lister.Id;
            manager.NewAd(ad);

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Account()
        {
            var manager = new DbManager(_connectionString);
            var lister = manager.GetByEmail(User.Identity.Name);

            var vm = new AccountViewModel
            {
                AllAds = manager.GetAllAds(),
                Lister = lister
            };

            foreach (Ad ad in vm.AllAds)
            {
                ad.Delete = ad.ListerId == lister.Id;
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            var manager = new DbManager(_connectionString);
            manager.Delete(id);

            return RedirectToAction("index");
        }
    }
}
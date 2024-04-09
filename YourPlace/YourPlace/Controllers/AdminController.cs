using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;
using YourPlace.Models.AdminModels;

namespace YourPlace.Controllers
{
    public class AdminController : Controller
    {
        private readonly HotelsServices _hotelsServices;
        public AdminController(HotelsServices hotelsServices)
        {
            _hotelsServices = hotelsServices;
        }

        private const string toAdminStartPage = "~/Views/Bulgarian/AdminViews/AdminStartPage.cshtml";
        public async Task<IActionResult> Index(int page = 1)
        {
            var hotelsPerPage = 4.0;
            var hotels = await _hotelsServices.ReadAllAsync();
            var paginatedHotels = hotels.Skip((page - 1) * (int)hotelsPerPage).Take((int)hotelsPerPage).ToList();
            ViewBag.Pages = Math.Ceiling((double)(hotels.Count / hotelsPerPage));
            ViewBag.CurrentPage = page;
            return View(toAdminStartPage, new AdminModel { Hotels = paginatedHotels });
        }
        public async Task<IActionResult> ShowNotVerifiedHotels(int page = 1)
        {
            try
            {
                var hotelsPerPage = 4.0;
                var hotels = await _hotelsServices.ReadAllAsync();
                hotels = hotels.Where(x => x.Verified == false).ToList();
                var paginatedHotels = hotels.Skip((page - 1) * (int)hotelsPerPage).Take((int)hotelsPerPage).ToList();
                ViewBag.Pages = Math.Ceiling((double)(hotels.Count / hotelsPerPage));
                ViewBag.CurrentPage = page;
                return View(toAdminStartPage, new AdminModel { Hotels = paginatedHotels });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;
using YourPlace.Models.AdminModels;
using YourPlace.Infrastructure.Data.Entities;

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
        private const string toVerificationPage = "~/Views/Bulgarian/AdminViews/VerificationPage.cshtml";
        public async Task<IActionResult> Index(int page = 1)
        {
            var hotelsPerPage = 4.0;
            var hotels = await _hotelsServices.AdminReadAllAsync();
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
                var hotels = await _hotelsServices.AdminReadAllAsync();
                hotels = hotels.Where(x => x.Verified == false).ToList();
                var paginatedHotels = hotels.Skip((page - 1) * (int)hotelsPerPage).Take((int)hotelsPerPage).ToList();
                ViewBag.Pages = Math.Ceiling((double)(hotels.Count / hotelsPerPage));
                ViewBag.CurrentPage = page;
                foreach(var hotel in  hotels)
                {
                    Console.WriteLine(hotel.ToString());
                }
                return View(toVerificationPage, new AdminModel { Hotels = paginatedHotels });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }
        public async Task<IActionResult> ShowVerifiedHotels(int page = 1)
        {
            try
            {
                var hotelsPerPage = 4.0;
                var hotels = await _hotelsServices.ReadAllAsync();
                hotels = hotels.Where(x => x.Verified == true).ToList();
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
        public async Task<IActionResult> Verify(int hotelID)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            hotel.Verified = true;
            await _hotelsServices.UpdateAsync(hotel);
            return RedirectToAction("ShowNotVerifiedHotels", "Admin");
        }
        public async Task<IActionResult> Decline(int hotelID)
        {
            await _hotelsServices.DeleteAsync(hotelID);
            return RedirectToAction("ShowNotVerifiedHotels", "Admin");
        }
        public async Task<IActionResult> DeleteHotel(int hotelID)
        {
            await _hotelsServices.DeleteAsync(hotelID);
            return RedirectToAction("Index", "Admin");
        }

    }
}

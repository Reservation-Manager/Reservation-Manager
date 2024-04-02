using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Models.ManagerModels;
using YourPlace.Models.Managers_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
namespace YourPlace.Controllers
{
    public class ManagerMenuController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly HotelsServices _hotelsServices;
        public ManagerMenuController(IWebHostEnvironment webHost, HotelsServices hotelsServices)
        {
            _webHost = webHost;
            _hotelsServices = hotelsServices;
        }

        private const string toManagerStartPage = "~/Views/Bulgarian/ManagerViews/StartPage.cshtml";
        private const string toAddHotelPage = "~/Views/Bulgarian/ManagerViews/AddHotel.cshtml";
        private const string toManagerHotels = "~/Views/Bulgarian/ManagerViews/ManagerHotels.cshtml";
        private const string toUploadImage = "~/Views/Bulgarian/ManagerViews/UploadImage.cshtml";

        public IActionResult Index(string firstName, string lastName)
        {
            return View(toManagerStartPage, new ManagerRegistrationModel { FirstName = firstName, LastName = lastName});
        }

        public IActionResult AddHotel()
        {
            return View(toUploadImage);
        }
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHotel([Bind("HotelName, Address, Town, Country, Rating, Details")] string hotelName, string address, string town, string country, double rating, string details, IFormFile image)
        {
            string mainImageURL = "";

            if (image != null && image.Length > 0)
            {
                // Save the uploaded image to the Images folder in wwwroot
                var uploadsDir = Path.Combine(_webHost.WebRootPath, "Images", "MainImages");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                var filePath = Path.Combine(uploadsDir, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                mainImageURL = "/Images/MainImages/" + uniqueFileName;
            }

            Hotel hotel = new Hotel(mainImageURL, hotelName, address, town, country, rating, details);
            List<Hotel> hotels = new List<Hotel>();
            hotels.Add(hotel);
            await _hotelsServices.CreateAsync(hotel);
            return View(toManagerHotels, new HotelCreateModel { ManagerHotels = hotels });
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imgfile)
        {
            var saveimg = Path.Combine(_webHost.WebRootPath, "Images/MainImages", imgfile.FileName);
            string imgext = Path.GetExtension(imgfile.FileName);
            if (imgext == ".jpg" || imgext == ".png")
            {
                using (var uploadimg = new FileStream(saveimg, FileMode.Create))
                {
                    await imgfile.CopyToAsync(uploadimg);
                    ViewData["Message"] = "The Selected File " + imgfile.FileName + " Is Saved Successfully ..!";
                }

                string imageUrl = imgfile.FileName; // Assuming Images folder is directly under wwwroot
                Console.WriteLine(imageUrl);
                return View(toAddHotelPage, new HotelCreateModel { MainImageURL = imageUrl });
            }
            else
            {
                ViewData["Message"] = "Only the image file .jpg & .png are allowed ...";
                return View(toAddHotelPage, new HotelCreateModel()); // Assuming you still want to return the view even if the file type is not supported.
            }
           
        }

    }
}

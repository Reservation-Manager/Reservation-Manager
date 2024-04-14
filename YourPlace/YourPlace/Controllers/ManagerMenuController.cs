using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Models.ManagerModels;
using YourPlace.Models.Managers_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using YourPlace.Infrastructure.Data.Enums;
using YourPlace.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace YourPlace.Controllers
{
    public class ManagerMenuController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly HotelsServices _hotelsServices;
        private readonly RoomServices _roomServices;
        private readonly HotelCategoriesServices _hotelCategoriesServices;
        private readonly UserServices _userServices;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterModel> _logger;

        public ManagerMenuController(
            IWebHostEnvironment webHost,
            HotelsServices hotelsServices,
            RoomServices roomServices,
            HotelCategoriesServices hotelCategoriesServices,
            UserServices userServices,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUserStore<User> userStore,
            IEmailSender emailSender,
            ILogger<RegisterModel> logger)
        {
            _webHost = webHost;
            _hotelsServices = hotelsServices;
            _roomServices = roomServices;
            _hotelCategoriesServices = hotelCategoriesServices;
            _userServices = userServices;
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailSender = emailSender;
            _logger = logger;
        }


        private const string toManagerStartPage = "~/Views/Bulgarian/ManagerViews/StartPage.cshtml";
        private const string toAddHotelPage = "~/Views/Bulgarian/ManagerViews/AddHotel.cshtml";
        private const string toManagerHotels = "~/Views/Bulgarian/ManagerViews/ManagerHotels.cshtml";
        private const string toUploadImage = "~/Views/Bulgarian/ManagerViews/UploadImage.cshtml";
        private const string toReceptionists = "~/Views/Bulgarian/ManagerViews/ReceptionistsManagement.cshtml";
        private const string toReceptionistsView = "~/Views/Bulgarian/ManagerViews/ReceptionistView.cshtml";
        private const string toReceptionistsViews = "~/Views/Bulgarian/ManagerViews/ReceptionistsView.cshtml";

        public IActionResult Index([Bind("ManagerID", "FirstName", "LastName")]string managerID, string firstName, string lastName)
        {
            Console.WriteLine(managerID);
            return View(toManagerStartPage, new HotelCreateModel { ManagerID = managerID, FirstName = firstName, LastName = lastName});
        }
        public IActionResult ReturnToStart([Bind("ManagerID, FirstName, LastName")] string managerID, string firstName, string lastName)
        {
            return View(toManagerStartPage, new HotelCreateModel { ManagerID = managerID, FirstName = firstName, LastName = lastName });
        }

        public IActionResult AddHotel([Bind("ManagerID", "FirstName", "LastName")] string managerID, string firstName, string lastName)
        {
            return View(toAddHotelPage, new HotelCreateModel { ManagerID = managerID, FirstName = firstName, LastName = lastName });
        }
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHotel([Bind("ManagerID, FirstName, LastName, HotelName, Address, Town, Country, Rating, Details, RoomsInHotel, Location, Tourism, Atmosphere, Company, Pricing")] string managerID, string hotelName, string address, string town, string country, double rating, string details, List<Room> roomsInHotel, IFormFile imgfile, Location location, Tourism tourism, Atmosphere atmosphere, Company company, Pricing pricing, string firstName, string lastName)
        {
            
            var saveimg = Path.Combine(_webHost.WebRootPath, "Images/MainImages", imgfile.FileName);
            string imgext = Path.GetExtension(imgfile.FileName);
            string imageUrl = "";
            if (imgext == ".jpg" || imgext == ".png")
            {
                using (var uploadimg = new FileStream(saveimg, FileMode.Create))
                {
                    await imgfile.CopyToAsync(uploadimg);
                }

                imageUrl = imgfile.FileName; 
                Console.WriteLine(imageUrl);
            }
            else
            {
                ViewData["Message"] = "Само файлове с разширение .jpg & .png са позволени ...";
                
            }

            Hotel hotel = new Hotel(imageUrl, hotelName, address, town, country, rating, details, managerID);
            List<Hotel> hotels = new List<Hotel>();
            hotels.Add(hotel);
            await _hotelsServices.CreateAsync(hotel);
            
            foreach(var room in roomsInHotel)
            {
                if(room.CountInHotel != 0)
                {
                    await _roomServices.CreateAsync(room);
                    Console.WriteLine(room.Type + " " + room.CountInHotel + " " + room.Price);
                }
            }
            Categories categories = new Categories(location, tourism, atmosphere, company, pricing, hotel.HotelID);
            await _hotelCategoriesServices.CreateAsync(categories);
       
            return View(toManagerHotels, new HotelCreateModel { ManagerHotels = hotels, ManagerID = managerID, FirstName = firstName, LastName = lastName });
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
        public async Task<IActionResult> ViewManagerHotels([Bind("ManagerID", "FirstName", "LastName")]string managerID, string firstName, string lastName)
        {
            if(managerID == null)
            {
                Console.WriteLine("MANAGER ID IS NULL.");
            }
            Console.WriteLine(managerID);
            List<Hotel> hotels = await _hotelsServices.AdminReadAllAsync();
            hotels = hotels.Where(x=>x.ManagerID == managerID).ToList();
            Console.WriteLine(managerID);
            return View(toManagerHotels, new HotelCreateModel { ManagerHotels = hotels, ManagerID = managerID, FirstName = firstName, LastName = lastName });
        }
        public async Task<IActionResult> ShowNotVerifiedHotels([Bind("ManagerID", "FirstName", "LastName")] string managerID, string firstName, string lastName)
        {
            List<Hotel> hotels = await _hotelsServices.AdminReadAllAsync();
            hotels = hotels.Where(x=>x.ManagerID==managerID).Where(x=>x.Verified == false).ToList();
            return View(toManagerHotels, new HotelCreateModel { ManagerHotels = hotels, ManagerID = managerID, FirstName = firstName, LastName = lastName });
        }

        public async Task<IActionResult> SetHotelID([Bind("ManagerID", "ReceptionistID", "FirstName", "LastName")] string managerID, string receptionistID, string firstName, string lastName)
        {
            Console.WriteLine("I AM IN THE SETHOTELID" + managerID);
            Console.WriteLine("I AM IN THE SETHOTELID" + receptionistID);
            List<Hotel> hotels = await _hotelsServices.AdminReadAllAsync();
            hotels = hotels.Where(x => x.ManagerID == managerID).Where(x => x.Verified == true).ToList();
            return View(toReceptionists, new HotelCreateModel { ManagerHotels = hotels, ManagerID = managerID, ReceptionistId = receptionistID, FirstName = firstName, LastName = lastName });
        }
        public async Task<IActionResult> Set([Bind("ManagerID", "ReceptionistID", "FirstName", "LastName", "HotelID")] string managerID, string receptionistID, string firstName, string lastName, int hotelID)
        {
            Console.WriteLine("HOTEL ID:" + hotelID);
            Console.WriteLine(managerID);
            Console.WriteLine(receptionistID);
            Console.WriteLine(firstName);
            User receptionist = await _userManager.FindByIdAsync(receptionistID);
            await _userServices.UpdateReceptionistAccountAsync(receptionistID, hotelID);
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            return View(toReceptionistsView, new HotelCreateModel { User = receptionist, ReceptionistId = receptionistID, ManagerID = managerID, HotelName = hotel.HotelName, FirstName = firstName, LastName = lastName });
        }

        public async Task<IActionResult> ViewAllVerifiedHotels([Bind("ManagerID", "FirstName", "LastName")]string managerID, string firstName, string lastName)
        {
            List<Hotel> hotels = await _hotelsServices.AdminReadAllAsync();
            hotels = hotels.Where(x => x.ManagerID == managerID).Where(x => x.Verified == true).ToList();
            return View(toReceptionists, new HotelCreateModel { ManagerHotels = hotels, ManagerID = managerID, FirstName = firstName, LastName = lastName });
        }
        public async Task<IActionResult> ViewAllReceptionistsForHotel([Bind("ManagerID", "HotelID", "FirstName", "LastName")] string managerID, int hotelID, string firstName, string lastName)
        {
           
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            var receptionists = await _userServices.ReadAllReceptionistsForHotelAsync(hotelID);
            return View(toReceptionistsViews, new HotelCreateModel { Receptionists = receptionists, ManagerID = managerID, HotelName = hotel.HotelName, FirstName = firstName, LastName = lastName });
        }
        
    }
    
}

using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;

namespace YourPlace.Controllers
{
    public class ReceptionistController1 : Controller
    {
        private readonly HotelsServices _hotelsServices;
        public ReceptionistController1(HotelsServices hotelsServices)
        {
            _hotelsServices = hotelsServices;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

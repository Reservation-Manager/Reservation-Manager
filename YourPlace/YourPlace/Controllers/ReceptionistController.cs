using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Models;

namespace YourPlace.Controllers
{
    public class ReceptionistController : Controller
    {
        private readonly HotelsServices _hotelsServices;
        private readonly ReservationServices _reservationServices;
        public ReceptionistController(HotelsServices hotelsServices, ReservationServices reservationServices)
        {
            _hotelsServices = hotelsServices;
            _reservationServices = reservationServices;

        }

        private const string toReceptionistStart = "~/Views/Bulgarian/ReceptionistViews/Index.cshtml";
        private const string toReservationView = "~/Views/Bulgarian/ReceptionistViews/ReservationView.cshtml";
        public async Task<IActionResult> Index()
        {
            List<Hotel> hotels = await _hotelsServices.ReadAllAsync();
            return View(toReceptionistStart, new ReservationModel { Hotels = hotels});
        }
        public async Task<IActionResult> ViewReservations([Bind("HotelID")] int hotelID)
        {
            var reservations = await _reservationServices.FindReservationsForHotel(hotelID);
            return View(toReservationView, new ReservationModel {Reservations = reservations});
        }
        public async Task<IActionResult> Verify(int reservationID)
        {
            Reservation reservation = await _reservationServices.ReadAsync(reservationID);
            reservation.Verified = true;
            await _reservationServices.UpdateAsync(reservation);
            return RedirectToAction("ViewReservations");
        }
        public async Task<IActionResult> Decline(int reservationID)
        {
            await _reservationServices.DeleteAsync(reservationID);
            return RedirectToAction("ViewReservations");
        }

    }
}

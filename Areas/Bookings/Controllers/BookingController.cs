using Microsoft.AspNetCore.Mvc;

namespace Bicks.Areas.Booking.Controllers
{
    [Area("Bookings")]
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CancelBooking()
        {
            return View();
        }

        public IActionResult EditBooking()
        {
            return View();
        }
    }
}

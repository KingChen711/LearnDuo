using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TutorDemand.Business;

namespace TutorDemand.Pages.Reservations
{
    public class IndexModel : PageModel
    {
        private readonly ReservationBusiness _reservationService;

        public IndexModel(ReservationBusiness reservationService)
        {
            _reservationService = reservationService;
        }

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public IActionResult OnGet()
        {
            var result = _reservationService.GetAll();
            if (result.Status == 1)
            {
                Reservations = (List<Reservation>)result.Data;
            }
            else
            {
                return RedirectToPage("Error");
            }

            return Page();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;


namespace TutorDemand.RazorWebApp.Pages.Reservation
{
    public class DeleteModel : PageModel
    {
        private readonly IReservationBusiness _reservationService;

        public DeleteModel(IReservationBusiness reservationService)
        {
            _reservationService = reservationService;
        }


        public async Task<IActionResult> OnGet(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var reservationResult = await _reservationService.GetAllAsync();

            if (reservationResult.Status == 1 && reservationResult.Data != null)
            {
                
                var reservationList = (List<Data.Entities.Reservation>)reservationResult.Data;
                var reservation = reservationList.First(r => r.ReservationId.Equals(id));
                var deleteResult = await _reservationService.DeleteAsync(reservation.ReservationId);

                if (deleteResult.Status == 1)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error deleting reservation.");
                    return Page();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Reservation not found.");
                return Page();
            }
        }
    }
}
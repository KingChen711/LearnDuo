using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;

namespace TutorDemand.RazorWebApp.Pages.Reservation;

public class DeleteModel : PageModel
{
    private readonly IReservationBusiness _reservationBusiness;

    public DeleteModel(IReservationBusiness reservationBusiness)
    {
        _reservationBusiness = reservationBusiness;

    }
    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        await _reservationBusiness.DeleteAsync(id);
        return RedirectToPage("/Reservation/Index");
    }
}


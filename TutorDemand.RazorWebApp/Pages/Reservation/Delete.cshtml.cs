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

    public async Task<IActionResult> OnGet(Guid id)
    {
        var reservationData = await _reservationBusiness.GetByIdAsync(id);
        if (reservationData.Data != null)
        {
            await _reservationBusiness.DeleteAsync(id);
            return RedirectToPage("/Reservation/Index");
        }
        return Page();
    }
    public void OnGet()
    {
        
    }
}
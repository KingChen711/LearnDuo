using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.Reservation;

namespace TutorDemand.RazorWebApp.Pages.Reservation;

public class UpdateModel : PageModel
{
    private readonly IReservationBusiness _reservationBusiness;

    public UpdateModel(IReservationBusiness reservationBusiness)
    {
        _reservationBusiness = reservationBusiness;
    }
    
    public void OnGet()
    {
        
    }

    [BindProperty] public ReservationUpdateDto UpdateRequest { get; set; } = null!;


    public async Task<IActionResult> OnPost(Guid id)
    {
        var reservations = await _reservationBusiness.GetAllAsync();
        var reseravtionData = (List<Data.Entities.Reservation>)reservations.Data!;
        var updatedReservation = reseravtionData.FirstOrDefault(r => r.ReservationId.Equals(id));
        if (updatedReservation is null)
        {
            return NotFound();
        }
        
        updatedReservation.ReservationStatus = UpdateRequest.ReservationStatus;
        updatedReservation.PaymentMethod = UpdateRequest.PaymentMethod;
        updatedReservation.PaymentStatus = UpdateRequest.PaymentStatus;

        var result = await _reservationBusiness.UpdateAsync(updatedReservation!);
        if (result.Status == 1)
        {
            return RedirectToPage("/Reservation/Index");
        }
        else
        {
            return RedirectToPage("/Error");
        }
    }

}
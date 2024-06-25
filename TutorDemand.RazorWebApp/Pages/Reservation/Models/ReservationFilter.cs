using TutorDemand.Data.Enums;

namespace TutorDemand.RazorWebApp.Pages.Reservation.Models;

public class ReservationFilter
{
    public decimal? PaidPrice { get; set; }
    public DateTime? CreateAt { get; set; }
    public string TutorName { get; set; }
    public string SubjectName { get; set; }
}
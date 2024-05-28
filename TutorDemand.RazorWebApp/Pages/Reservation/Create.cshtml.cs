using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Enums;

namespace TutorDemand.Pages
{
    public class CreateReservationModel : PageModel
    {
        [BindProperty]
        public ReservationCreateDto Reservation { get; set; } = new ReservationCreateDto();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Thực hiện logic lưu trữ dữ liệu
            // ...

            return RedirectToPage("Success"); // Redirect đến trang thành công
        }
    }
}
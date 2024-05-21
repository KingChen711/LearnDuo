using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.Tutor;

namespace TutorDemand.RazorWebApp.Pages.Tutor
{
    public class CreateModel : PageModel
    {
        private readonly ITutorBusiness tutorBusiness;

        public CreateModel(ITutorBusiness tutorBusiness)
        {
            this.tutorBusiness = tutorBusiness;
        }
        [BindProperty]
        public TutorAddDto Tutor { get; set; }

        public async void OnPost()
        {
            Tutor.TutorId = Guid.NewGuid();
           await tutorBusiness.Create(Tutor);
        }
    }
}

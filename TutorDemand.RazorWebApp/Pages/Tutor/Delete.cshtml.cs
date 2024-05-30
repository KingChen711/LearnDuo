using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Tutor;

namespace TutorDemand.RazorWebApp.Pages.Tutor
{
    public class DeleteModel : PageModel
    {
        private readonly ITutorBusiness _tutorBusiness;
        private readonly IMapper _mapper;

        public DeleteModel(ITutorBusiness tutorBusiness, IMapper mapper)
        {
            _tutorBusiness = tutorBusiness;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            IBusinessResult businessResult = await _tutorBusiness.FindOneAsync(x => x.TutorId.Equals(id));
            if (businessResult != null)
            {
                await _tutorBusiness.DeleteAsync(id);
               return RedirectToPage("/tutor/list");
            }
            return Page();
        }
    }
}

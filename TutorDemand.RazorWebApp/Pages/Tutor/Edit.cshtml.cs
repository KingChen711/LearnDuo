using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Tutor;

namespace TutorDemand.RazorWebApp.Pages.Tutor
{
    public class EditModel : PageModel
    {
        private readonly ITutorBusiness _tutorBusiness;
        private readonly IMapper _mapper;

        public EditModel(ITutorBusiness tutorBusiness, IMapper mapper)
        {
            _tutorBusiness = tutorBusiness;
            _mapper = mapper;
        }

        [BindProperty] public TutorDto Tutor { get; set; }
        [BindProperty] public string idEdit { get; set; }

        public async Task OnGet(Guid id)
        {
            IBusinessResult businessResult = await _tutorBusiness.FindOneAsync(x => x.TutorId.Equals(id));
            if (businessResult != null)
            {
                Tutor = _mapper.Map<TutorDto>(businessResult.Data);
            }
        }

        public async Task<IActionResult> OnPost()
        {
            Tutor.TutorId = Guid.Parse(idEdit);
            await _tutorBusiness.UpdateAsync(Tutor);
            return RedirectToPage("/tutor/list");
        }
    }
}

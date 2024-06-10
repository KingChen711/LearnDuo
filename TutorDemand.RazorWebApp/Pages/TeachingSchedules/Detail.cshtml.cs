using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
using TutorDemand.Data.Entities;

namespace TutorDemand.RazorWebApp.Pages.TeachingSchedules
{
    public class Detail : PageModel
    {
        private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;

        public Detail(
            ITeachingScheduleBusiness teachingScheduleBusiness
        )
        {
            _teachingScheduleBusiness = teachingScheduleBusiness;
        }

        public TeachingSchedule TeachingSchedule { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var result = await _teachingScheduleBusiness.GetDetail((Guid)id);

            if (result.Status != Const.SUCCESS_READ_CODE)
            {
                return NotFound();
            }

            TeachingSchedule = (result.Data as TeachingSchedule)!;

            return Page();
        }
    }
}
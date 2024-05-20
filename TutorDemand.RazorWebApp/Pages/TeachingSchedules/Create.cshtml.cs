using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.TeachingSchedule;

namespace TutorDemand.RazorWebApp.Pages.TeachingSchedules
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger;
        private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;

        public Create(ILogger<Create> logger, ITeachingScheduleBusiness teachingScheduleBusiness)
        {
            _logger = logger;
            _teachingScheduleBusiness = teachingScheduleBusiness;
        }

        [BindProperty] public TeachingScheduleCreationDto TeachingSchedule { get; set; } = null!;

        [BindProperty] public List<string> SelectedDays { get; set; } = [];

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var learnDays = string.Join(",", SelectedDays);

            if (learnDays.IsNullOrEmpty())
            {
                ModelState.AddModelError("TeachingSchedule.LearnDays", "Các ngày học trong tuần bắt buộc");
                return Page();
            }

            TeachingSchedule.LearnDays = learnDays;
            await _teachingScheduleBusiness.Create(TeachingSchedule);

            return Page();
        }
    }
}
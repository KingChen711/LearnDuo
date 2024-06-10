using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Entities;

namespace TutorDemand.RazorWebApp.Pages.TeachingSchedules
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger;
        private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly ITutorBusiness _tutorBusiness;
        private readonly ISlotBusiness _slotBusiness;

        public Create(ILogger<Create> logger, IMapper mapper, NET1704_221_5_TutorDemandContext context,
            ITeachingScheduleBusiness teachingScheduleBusiness, ISubjectBusiness subjectBusiness,
            ITutorBusiness tutorBusiness, ISlotBusiness slotBusiness)
        {
            _logger = logger;
            _teachingScheduleBusiness = teachingScheduleBusiness;
            _subjectBusiness = subjectBusiness;
            _tutorBusiness = tutorBusiness;
            _slotBusiness = slotBusiness;
        }

        public IEnumerable<SelectListItem> SelectListSubject = [];
        public IEnumerable<SelectListItem> SelectListTutor = [];
        public IEnumerable<SelectListItem> SelectListSlot = [];

        [BindProperty] public TeachingScheduleMutationDto TeachingSchedule { get; set; } = null!;
        [BindProperty] public List<string> SelectedDays { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            var subjectsResultTask = _subjectBusiness.GetAllAsync();
            var tutorsResultTask = _tutorBusiness.GetAllAsync();
            var slotsResultTask = _slotBusiness.GetAllAsync();

            var results = await Task.WhenAll(subjectsResultTask, tutorsResultTask, slotsResultTask);

            if (results.Any(r => r.Status != Const.SUCCESS_READ_CODE))
            {
                return RedirectToPage("/Error");
            }

            SelectListSubject = results[0].Data.Adapt<List<Data.Entities.Subject>>()
                .Select(x => new SelectListItem(x.Name, x.SubjectId.ToString())).ToList();
            SelectListTutor = results[1].Data.Adapt<List<Data.Entities.Tutor>>()
                .Select(x => new SelectListItem(x.Fullname, x.TutorId.ToString())).ToList();
            SelectListSlot = results[2].Data.Adapt<List<Data.Entities.Slot>>()
                .Select(x => new SelectListItem(x.SlotName, x.SlotId.ToString())).ToList();

            return Page();
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
            await _teachingScheduleBusiness.CreateAsync(TeachingSchedule);

            return RedirectToPage("/TeachingSchedules/Index");
        }
    }
}
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

namespace TutorDemand.RazorWebApp.Pages.TeachingSchedules;

public class Update : PageModel
{
    private readonly ILogger<Create> _logger;
    private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;
    private readonly ISubjectBusiness _subjectBusiness;
    private readonly ITutorBusiness _tutorBusiness;
    private readonly ISlotBusiness _slotBusiness;

    public Update(ILogger<Create> logger, IMapper mapper)
    {
        _logger = logger;
        _teachingScheduleBusiness ??= new TeachingScheduleBusiness();
        _subjectBusiness ??= new SubjectBusiness(mapper);
        _tutorBusiness ??= new TutorBusiness();
        _slotBusiness ??= new SlotBusiness();
    }

    public IEnumerable<SelectListItem> SelectListSubject = [];
    public IEnumerable<SelectListItem> SelectListTutor = [];
    public IEnumerable<SelectListItem> SelectListSlot = [];

    [BindProperty] public TeachingScheduleMutationDto TeachingSchedule { get; set; } = null!;
    [BindProperty] public List<string> SelectedDays { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var result = await _teachingScheduleBusiness.FindOne(t => t.TeachingScheduleId == id);

        if (result.Status != Const.SUCCESS_READ_CODE)
        {
            return NotFound();
        }

        TeachingSchedule = result.Data.Adapt<TeachingScheduleMutationDto>();
        SelectedDays = TeachingSchedule.LearnDays!.Split(',').ToList();

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
        await _teachingScheduleBusiness.Create(TeachingSchedule);

        return RedirectToPage("/TeachingSchedules/Index");
    }
}
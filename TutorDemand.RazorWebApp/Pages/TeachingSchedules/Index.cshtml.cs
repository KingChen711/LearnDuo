using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
using TutorDemand.Data.Dtos;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Entities;

namespace TutorDemand.RazorWebApp.Pages.TeachingSchedules;

public class Index : PageModel
{
    private readonly ILogger<Create> _logger;
    private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;

    public Index(ILogger<Create> logger, ITeachingScheduleBusiness teachingScheduleBusiness)
    {
        _logger = logger;
        _teachingScheduleBusiness = teachingScheduleBusiness;
    }

    public string SearchSubject { get; set; } = "";
    public string SearchTutor { get; set; } = "";
    public string SearchSlot { get; set; } = "";


    public PagingMetadata Metadata { get; set; } = null!;
    public PagedList<TeachingSchedule> TeachingSchedules { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int pageSize = 5, int pageNumber = 1, string searchSubject = "",
        string searchTutor = "",string searchSlot = "")
    {
        SearchSubject = searchSubject;
        SearchTutor = searchTutor;
        SearchSlot = searchSlot;

        var queryParams = new QueryTeachingScheduleDto
            { PageNumber = pageNumber, PageSize = pageSize, SearchSubject = SearchSubject, SearchTutor = SearchTutor, SearchSlot = SearchSlot };

        var teachingSchedulesResult = await _teachingScheduleBusiness.GetTeachingSchedulesAsync(queryParams);

        if (teachingSchedulesResult.Status != Const.SUCCESS_READ_CODE)
        {
            return RedirectToPage("/Error");
        }

        TeachingSchedules = (PagedList<TeachingSchedule>)teachingSchedulesResult.Data!;
        Metadata = TeachingSchedules.MetaData;

        return Page();
    }

    //DeleteAsync teaching schedules
    public async Task<IActionResult> OnPostAsync(string? type, Guid? id)
    {
        if (id is null || type is null || type != "delete")
        {
            return RedirectToPage("/Error");
        }

        var teachingScheduleResult = await _teachingScheduleBusiness.FindOneAsync(ts => ts.TeachingScheduleId == id);

        if (teachingScheduleResult.Status != Const.FAIL_READ_CODE)
        {
            return NotFound();
        }

        var deleteTeachingScheduleResult = await _teachingScheduleBusiness.DeleteAsync((Guid)id);

        if (deleteTeachingScheduleResult.Status != Const.FAIL_CREATE_CODE)
        {
            return RedirectToPage("/Error");
        }

        return RedirectToPage("/teachingschedules");
    }
}
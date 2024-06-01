using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business;
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

    public Index(ILogger<Create> logger)
    {
        _logger = logger;
        _teachingScheduleBusiness ??= new TeachingScheduleBusiness();
    }

    public PagingMetadata Metadata { get; set; } = null!;
    public PagedList<TeachingSchedule> TeachingSchedules { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int pageSize = 5, int pageNumber = 1)
    {
        var queryParams = new QueryTeachingScheduleDto { PageNumber = pageNumber, PageSize = pageSize };

        var teachingSchedulesResult = await _teachingScheduleBusiness.GetTeachingSchedules(queryParams);

        if (teachingSchedulesResult.Status != Const.SUCCESS_READ_CODE)
        {
            return RedirectToPage("/Error");
        }

        TeachingSchedules = (PagedList<TeachingSchedule>)teachingSchedulesResult.Data!;
        Metadata = TeachingSchedules.MetaData;

        return Page();
    }

    //Delete teaching schedules
    public async Task<IActionResult> OnPostAsync(string? type, Guid? id)
    {
        if (id is null || type is null || type != "delete")
        {
            return RedirectToPage("/Error");
        }

        var teachingScheduleResult = await _teachingScheduleBusiness.FindOne(ts => ts.TeachingScheduleId == id);

        if (teachingScheduleResult.Status != Const.FAIL_READ_CODE)
        {
            return NotFound();
        }

        var deleteTeachingScheduleResult = await _teachingScheduleBusiness.Delete((Guid)id);

        if (deleteTeachingScheduleResult.Status != Const.FAIL_CREATE_CODE)
        {
            return RedirectToPage("/Error");
        }

        return RedirectToPage("/teachingschedules");
    }
}
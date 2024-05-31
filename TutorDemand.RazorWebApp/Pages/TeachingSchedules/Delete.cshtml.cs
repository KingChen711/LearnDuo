using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;

namespace TutorDemand.RazorWebApp.Pages.TeachingSchedules;

public class Delete : PageModel
{
    private readonly ILogger<Create> _logger;
    private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;

    public Delete(ILogger<Create> logger)
    {
        _logger = logger;
        _teachingScheduleBusiness ??= new TeachingScheduleBusiness();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null)
        {
            return RedirectToPage("/Error");
        }

        var teachingScheduleResult = await _teachingScheduleBusiness.FindOne(ts => ts.TeachingScheduleId == id);

        if (teachingScheduleResult.Status != Const.SUCCESS_READ_CODE)
        {
            return NotFound();
        }

        var deleteTeachingScheduleResult = await _teachingScheduleBusiness.Delete((Guid)id);

        if (deleteTeachingScheduleResult.Status != Const.SUCCESS_DELETE_CODE)
        {
            return RedirectToPage("/Error");
        }

        return RedirectToPage("/teachingschedules/index");
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.RazorWebApp.Models;

namespace TutorDemand.RazorWebApp.Pages.Subject
{
    public class UpdateModel : PageModel
    {
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UpdateModel(ISubjectBusiness subjectBusiness,
            IMapper mapper,
            IOptionsMonitor<AppSettings> monitor)
        {
            _subjectBusiness = subjectBusiness;
            _mapper = mapper;
            _appSettings = monitor.CurrentValue;
        }

        [BindProperty] public SubjectDto SubjectDto { get; set; } = null!;


        public async Task OnGet(Guid subjectId)
        {
            if (subjectId != Guid.Empty)
            {
                var businessResult = await _subjectBusiness.GetByIdAsync(subjectId);

                if (businessResult.Status == Const.SUCCESS_READ_CODE)
                {
                    SubjectDto = _mapper.Map<SubjectDto>(businessResult.Data);
                }
            }
        }

        public async Task<IActionResult> OnPostEdit()
        {
            try
            {
                var businessResult = await _subjectBusiness.UpdateAsync(SubjectDto);

                if (businessResult.Status == Const.SUCCESS_UPDATE_CODE)
                {
                    ViewData["Notification"] = new Notification
                    {
                        Message = "UpdateAsync subject successfully",
                        Type = Data.Enums.NotificationType.Sucess
                    };
                }
                else
                {
                    ViewData["Notification"] = new Notification
                    {
                        Message = businessResult.Message!,
                        Type = Data.Enums.NotificationType.Error
                    };
                }

                return Page();
            }
            catch (Exception)
            {
                ViewData["Notification"] = new Notification
                {
                    Message = "Something went wrong",
                    Type = Data.Enums.NotificationType.Error
                };
                throw;
            }
        }
    }
}
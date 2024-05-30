using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Utils;
using TutorDemand.RazorWebApp.Models;

namespace TutorDemand.RazorWebApp.Pages.Subject
{
    public class CreateModel : PageModel
    {
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CreateModel(ISubjectBusiness subjectBusiness,
            IMapper mapper,
            IOptionsMonitor<AppSettings> monitor)
        {
            _subjectBusiness = subjectBusiness;
            _mapper = mapper;
            _appSettings = monitor.CurrentValue;
        }

        [BindProperty] public SubjectAddDto SubjectAddDto { get; set; } = null!;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Process create subject...

            // Subject id 
            SubjectAddDto.SubjectId = Guid.NewGuid();

            // Business result 
            var businessResult = await _subjectBusiness.CreateAsync(
                _mapper.Map<SubjectDto>(SubjectAddDto)); // Map subject request obj into subject DTO

            if (businessResult.Status == Const.SUCCESS_CREATE_CODE)
            {
                var notification = new Notification()
                {
                    Message = "Create subject successfully",
                    Type = Data.Enums.NotificationType.Sucess
                };

                TempData["Notification"] = JsonSerializer.Serialize(notification);

                // Count total subject to forward user to last page, where new subject has been added
                businessResult = await _subjectBusiness.GetAllAsync();
                var subjectEntities = _mapper.Map<List<SubjectDto>>(businessResult.Data);
                var totalPage = (int)Math.Ceiling((double)subjectEntities.Count / _appSettings.PageSize);

                return RedirectToPage($"/subject/list", new { PageIndex = totalPage });
            }


            return Page();
        }
    }
}
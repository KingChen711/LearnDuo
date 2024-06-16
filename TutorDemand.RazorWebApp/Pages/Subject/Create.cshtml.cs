using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data.Dtos.Slot;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Utils;
using TutorDemand.RazorWebApp.Models;

namespace TutorDemand.RazorWebApp.Pages.Subject
{
    public class CreateModel : PageModel
    {
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly ITutorBusiness _tutorBusiness;
        private readonly ISlotBusiness _slotBusiness;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CreateModel(ISubjectBusiness subjectBusiness,
            ISlotBusiness slotBusiness,
            ITutorBusiness tutorBusiness,
            IMapper mapper,
            IOptionsMonitor<AppSettings> monitor)
        {
            _subjectBusiness = subjectBusiness;
            _tutorBusiness = tutorBusiness;
            _slotBusiness = slotBusiness;
            _mapper = mapper;
            _appSettings = monitor.CurrentValue;
        }

        [BindProperty] public SubjectAddDto SubjectAddDto { get; set; } 
        [BindProperty] public List<TutorDto> Tutors { get; set; }
        [BindProperty] public List<SlotDto> Slots { get; set; }

        public async Task OnGetAsync()
        {
            await RetrieveTutorsAndSlotsAsync();
        }

        private async Task RetrieveTutorsAndSlotsAsync()
        {
            IBusinessResult result = null!;
            // Retrieve tutors
            result = await _tutorBusiness.GetAllAsync();
            Tutors = _mapper.Map<List<TutorDto>>(result.Data);

            // Retrieve slots
            result = await _slotBusiness.GetAllAsync();
            Slots = _mapper.Map<List<SlotDto>>(result.Data);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IBusinessResult result = null!;
            if (!ModelState.IsValid)
            {
                await RetrieveTutorsAndSlotsAsync();

                if (String.IsNullOrEmpty(Request.Form["start-date"]))
                {
                    ModelState.AddModelError("SubjectAddDto.StartDate", $"Start date is required");
                }
                
                if(String.IsNullOrEmpty(Request.Form["end-date"]))
                {
                    ModelState.AddModelError("SubjectAddDto.EndDate", $"End date is required");
                }

                return Page();
            }

            // Check exist Code
            result = await _subjectBusiness.GetWithConditionAysnc(x => x.SubjectCode.Equals(SubjectAddDto.SubjectCode));
            if(result.Status == Const.SUCCESS_READ_CODE)
            {
                await RetrieveTutorsAndSlotsAsync();
                ModelState.AddModelError("SubjectAddDto.SubjectCode", $"Subject code already exist");
                return Page();
            }

            // Process create subject...

            // Subject id 
            SubjectAddDto.SubjectId = Guid.NewGuid();
            // Format cost price
            var price = SubjectAddDto.CostPrice.ToString().Replace('.', ' ').Replace(" ", string.Empty);
            SubjectAddDto.CostPrice = Decimal.Parse(price);
            // Format start/end date
            SubjectAddDto.StartDate = DateTime.Parse(Request.Form["start-date"]!);
            SubjectAddDto.EndDate = DateTime.Parse(Request.Form["end-date"]!);


            var subjectDto = _mapper.Map<SubjectDto>(SubjectAddDto);
            if (SubjectAddDto.TutorIds != null
                && SubjectAddDto.TutorIds.Any())
            {
                var slotId = Request.Form["slot"];
                if (String.IsNullOrEmpty(slotId))
                {
                    await RetrieveTutorsAndSlotsAsync();

                    ModelState.AddModelError("SubjectAddDto.SlotId", "Please select slot for tutor(s)");
                    return Page();
                }

                foreach(var tId in  SubjectAddDto.TutorIds)
                {
                    subjectDto.TeachingSchedules.Add(new()
                    {
                        StartDate = DateOnly.Parse(SubjectAddDto.StartDate.Value.Date.ToString("yyyy-MM-dd")!),
                        EndDate = DateOnly.Parse(SubjectAddDto.EndDate.Value.Date.ToString("yyyy-MM-dd")!),
                        MeetRoomCode = TeachingScheduleHelper.GenerateMeetRoomCode(),
                        RoomPassword = "@Password123",
                        LearnDays = TeachingScheduleHelper.GenerateRandomWeekdays(),
                        PaidPrice = int.Parse(SubjectAddDto.CostPrice.ToString()),
                        SlotId = Guid.Parse(slotId!),
                        SubjectId = SubjectAddDto.SubjectId,
                        TutorId = Guid.Parse(tId),
                        CreationDate = DateTime.Now,
                        LastUpdated = DateTime.Now
                    }); 
                }
            }
            // Create new subject
            result = await _subjectBusiness.CreateAsync(subjectDto); // Map subject request obj into subject DTO

            if (result.Status == Const.SUCCESS_CREATE_CODE)
            {
                var notification = new Notification()
                {
                    Message = "Create subject successfully",
                    Type = Data.Enums.NotificationType.Sucess
                };

                TempData["Notification"] = JsonSerializer.Serialize(notification);

                // Count total subject to forward user to last page, where new subject has been added
                result = await _subjectBusiness.GetAllAsync();
                var subjectEntities = _mapper.Map<List<SubjectDto>>(result.Data);
                var totalPage = (int)Math.Ceiling((double)subjectEntities.Count / _appSettings.PageSize);

                return RedirectToPage($"/subject/list", new { PageIndex = totalPage });
            }

            await RetrieveTutorsAndSlotsAsync();
            return Page();
        }
    }
}
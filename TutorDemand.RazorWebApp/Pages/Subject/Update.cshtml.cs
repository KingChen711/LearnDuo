using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.DAO;
using TutorDemand.Data.Dtos.Slot;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.RazorWebApp.Models;

namespace TutorDemand.RazorWebApp.Pages.Subject
{
    public class UpdateModel : PageModel
    {
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly ITutorBusiness _tutorBusiness;
        private readonly ISlotBusiness _slotBusiness;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UpdateModel(ISubjectBusiness subjectBusiness,
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

        //[BindProperty] public SubjectUpdateDto SubjectUpdateDto { get; set; }
        [BindProperty] public SubjectUpdateDto SubjectUpdateDto { get; set; }
        [BindProperty] public List<TutorDto> Tutors { get; set; }
        [BindProperty] public List<SlotDto> Slots { get; set; }

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

        public async Task OnGetAsync(Guid subjectId)
        {
            if (subjectId != Guid.Empty)
            {
                await RetrieveTutorsAndSlotsAsync();

                var businessResult = await _subjectBusiness.GetByIdAsync(subjectId);

                if (businessResult.Status == Const.SUCCESS_READ_CODE)
                {
                    var subjectDto = _mapper.Map<SubjectDto>(businessResult.Data);
                    SubjectUpdateDto = _mapper.Map<SubjectUpdateDto>(subjectDto);
                }
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await RetrieveTutorsAndSlotsAsync();

                    if (String.IsNullOrEmpty(Request.Form["start-date"]))
                    {
                        ModelState.AddModelError("SubjectUpdateDto.StartDate", $"Start date is required");
                    }

                    if (String.IsNullOrEmpty(Request.Form["end-date"]))
                    {
                        ModelState.AddModelError("SubjectUpdateDto.EndDate", $"End date is required");
                    }

                    return Page();
                }

                // Format cost price
                var price = SubjectUpdateDto.CostPrice.ToString().Replace('.', ' ').Replace(" ", string.Empty);
                SubjectUpdateDto.CostPrice = Decimal.Parse(price);
                // Format start/end date
                SubjectUpdateDto.StartDate = DateTime.Parse(Request.Form["start-date"]!);
                SubjectUpdateDto.EndDate = DateTime.Parse(Request.Form["end-date"]!);

                var businessResult = await _subjectBusiness.UpdateAsync(_mapper.Map<SubjectDto>(SubjectUpdateDto));

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
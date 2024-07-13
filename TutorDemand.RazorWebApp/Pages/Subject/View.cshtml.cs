using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;
using TutorDemand.RazorWebApp.Models;

namespace TutorDemand.RazorWebApp.Pages.Subject
{
    public class ViewModel : PageModel
    {
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly IMapper _mapper;
        private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;
        private readonly IReservationBusiness _reservationBusiness;

        public ViewModel(ISubjectBusiness subjectBusiness,
            IMapper mapper,
            ITeachingScheduleBusiness teachingScheduleBusiness,
            IReservationBusiness reservationBusiness)
        {
            _subjectBusiness = subjectBusiness;
            _mapper = mapper;
            _teachingScheduleBusiness = teachingScheduleBusiness;
            _reservationBusiness = reservationBusiness;
        }


        public SubjectDto Subject { get; set; } = null!;
        public List<TutorDto> Tutors { get; set; } = new List<TutorDto>();
        public List<TeachingScheduleDto> TeachingSchedules { get; set; } = new List<TeachingScheduleDto>();
        // Related subjects
        public List<SubjectDto> OtherSubjects { get; set; }

        public async Task OnGetAsync(Guid subjectId)
        {
            var businessResult = await _subjectBusiness.GetWithConditionAysnc(x =>
                // Building query to access data
                x.SubjectId.ToString().ToUpper().Equals(subjectId.ToString().ToUpper()),
                null!, // Without filter
                "TeachingSchedules"); // Include teaching schedule 

            if (businessResult.Status == Const.SUCCESS_READ_CODE)
            {
                var subjects = _mapper.Map<List<SubjectDto>>(businessResult.Data);


                if (subjects.Any())
                {
                    Subject = subjects.First();

                    // Get related subjects
                    var random = new Random();
                    businessResult = await _subjectBusiness.GetWithConditionAysnc(x =>
                        x.SubjectId != Subject.SubjectId, null!, null!);
                    // Map business result to List<SubjectDto>
                    OtherSubjects = _mapper.Map<List<SubjectDto>>(businessResult.Data);
                }
                else return; // Notification later

                // Get all tutor 
                var teachingSchedule = Subject.TeachingSchedules.ToList();
                var teachingScheduleIds = teachingSchedule.Select(x => x.Id).ToList();
                businessResult = await _teachingScheduleBusiness.GetWithConditionAsync(x =>
                    // Get all teaching schedule 
                    teachingScheduleIds.Contains(x.Id),
                    null!, // Without filter
                    "Tutor"); // Include tutor 

                if (businessResult.Status == Const.SUCCESS_READ_CODE)
                {
                    var teachingSchedules = _mapper.Map<List<TeachingScheduleDto>>(businessResult.Data);

                    foreach (var ts in teachingSchedules)
                    {
                        Tutors.Add(ts.Tutor);
                        TeachingSchedules.Add(ts);
                    }
                }

                // Check if customer already learn this subject
                var customer = SessionHelpers.GetObjectFromJson<Customer>(HttpContext.Session, "Customer");
                if (customer != null)
                {
                    // Check if customer already learn this subject
                    var existReservationResult = await _reservationBusiness.GetWithConditionAysnc(x =>
                        x.CustomerId.Equals(customer.CustomerId));

                    if (existReservationResult.Status == Const.SUCCESS_READ_CODE)
                    {
                        // Map reservations of customer
                        var existReservations = _mapper.Map<List<ReservationDetails>>(existReservationResult.Data);

                        // Get all schedules of customer
                        var customerScheduleIds = existReservations.Select(x => x.TeachingSchedule.SubjectId.ToString()).ToList();

                        // Check customer already learn this subject
                        if (customerScheduleIds.Contains(subjectId.ToString()))
                        {
                            TempData["SubjectEnrolled"] = "True";
                        }
                    }
                }
            }
        }
    }
}

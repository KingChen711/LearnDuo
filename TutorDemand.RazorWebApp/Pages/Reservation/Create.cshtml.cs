using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Enums;

namespace TutorDemand.RazorWebApp.Pages.Reservation
{
    public class CreateReservationModel : PageModel
    {
        private readonly IReservationBusiness _reservationBusiness;
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly ITutorBusiness _tutorBusiness;
        private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;
        private readonly IMapper _mapper;
        private readonly ICustomerBusiness _customerBusiness;

        public SubjectDto Subject { get; set; }
        public List<TutorDto> Tutors { get; set; } = new List<TutorDto>();
        public List<TeachingScheduleDto> TeachingSchedules { get; set; } = new List<TeachingScheduleDto>();

        [BindProperty]
        public ReservationCreateDto Reservation { get; set; } = new ReservationCreateDto();

        public CreateReservationModel(IReservationBusiness reservationBusiness,
            ISubjectBusiness subjectBusiness, ITutorBusiness tutorBusiness,
            ITeachingScheduleBusiness teachingScheduleBusiness, IMapper mapper,
            ICustomerBusiness customerBusiness)
        {
            _reservationBusiness = reservationBusiness;
            _subjectBusiness = subjectBusiness;
            _tutorBusiness = tutorBusiness;
            _teachingScheduleBusiness = teachingScheduleBusiness;
            _mapper = mapper;
            _customerBusiness = customerBusiness;

        }

        public async Task<IActionResult> OnGet(Guid subjectId)
        {
            var subjects = await _subjectBusiness.GetWithConditionAysnc(x => x.SubjectId.Equals(subjectId), null, "");
            if (subjects.Data is null)
            {
                return RedirectToPage("/Error");
            }
            var subjectData = ((List<Data.Entities.Subject>)subjects.Data!).First();
            Subject = _mapper.Map<SubjectDto>(subjectData);

            var teachingSchedule = await _teachingScheduleBusiness.GetAllAsync();
            if (teachingSchedule.Data is null)
            {
                return RedirectToPage("/Error");
            }

            var teachingScheduleDatas = ((List<TeachingSchedule>)teachingSchedule.Data!).Where(
                t => t.SubjectId.Equals(subjectData.SubjectId));
            foreach (var teachingScheduleData in teachingScheduleDatas)
            {
                var tutorId = teachingScheduleData.TutorId;
                var tutor = await _tutorBusiness.FindOneAsync(x => x.TutorId.Equals(tutorId));
                if (tutor.Data is null)
                {
                    return RedirectToPage("/Error");
                }
                Tutors.Add(_mapper.Map<TutorDto>((Data.Entities.Tutor)tutor.Data!));
            }
            return Page();
        }

        public async Task<IActionResult> OnGetSchedules(Guid tutorId, Guid subjectId)
        {
            var schedules = await _teachingScheduleBusiness.GetWithConditionAsync(
                x => x.TutorId.Equals(tutorId) && x.SubjectId.Equals(subjectId), null, "");
            if (schedules.Data is null)
            {
                return new JsonResult(new List<TeachingScheduleDto>());
            }
            return new JsonResult(_mapper.Map<List<TeachingScheduleDto>>(schedules.Data));
        }

        public async Task<IActionResult> OnPost(Guid subjectId)
        {

            var req = Request.Form;
            var customer = await _customerBusiness.GetAllAsync();
            var currentCustomer = ((List<Customer>)customer.Data!).First();
            Reservation.CustomerId = currentCustomer.CustomerId;
            Reservation.CreatedAt = DateTime.Now;
            Reservation.ReservationStatus = ReservationStatus.Processing.ToString();
            Reservation.PaymentStatus = PaymentStatus.Unpaid.ToString();
            Reservation.TeachingScheduleId = Guid.Parse(Request.Form["Reservation.TeachingScheduleId"]);
            
            Reservation.PaidPrice = int.Parse(Request.Form["Reservation.CostPrice"]);

            await _reservationBusiness.CreateAsync(_mapper.Map<Data.Entities.Reservation>(Reservation));
            return RedirectToPage("/Reservation/Index");
        }
    }
}

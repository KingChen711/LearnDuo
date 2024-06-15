using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.RazorWebApp.Pages.Reservation;

public class ViewModel : PageModel
{
    private readonly IReservationBusiness _reservationBusiness;
    private readonly ITeachingScheduleBusiness _teachingscheduleBusiness;
    private readonly ITutorBusiness _tutorBusiness;
    private readonly IMapper _mapper;
    private readonly ISubjectBusiness _subjectBusiness;

    public ViewModel(IReservationBusiness reservationBusiness, ITeachingScheduleBusiness teachingScheduleBusiness,
        ITutorBusiness tutorBusiness, ISubjectBusiness subjectBusiness, IMapper mapper)
    {
        _reservationBusiness = reservationBusiness;
        _teachingscheduleBusiness = teachingScheduleBusiness;
        _tutorBusiness = tutorBusiness;
        _mapper = mapper;
        _subjectBusiness = subjectBusiness;
    }

    public ReservationDetails ReservationDetail { get; set; }
    public TutorDto TutorDetail { get; set; }
    public TeachingScheduleDto ScheduleDto { get; set; }


    public async Task<IActionResult> OnGet(Guid reservationId)
    {
        var reservation = await _reservationBusiness.GetAllAsync();
        if (reservation.Data is null)
        {
            return RedirectToPage("/Error");
        }

        var reservationData = ((List<Data.Entities.Reservation>)reservation.Data!).First(x=> x.ReservationId
            .Equals(reservationId));
        ReservationDetail = _mapper.Map<ReservationDetails>(reservationData);

        var teachingSchedule =
            await _teachingscheduleBusiness.FindOneAsync(x =>
                x.TeachingScheduleId.Equals(reservationData.TeachingScheduleId));
        if (teachingSchedule.Data is null)
        {
            return RedirectToPage("/Error");
        }

        var teachingScheduleData = (TeachingSchedule)teachingSchedule.Data!;
        ScheduleDto = _mapper.Map<TeachingScheduleDto>(teachingScheduleData);
        var tutor = await _tutorBusiness.FindOneAsync(x => x.TutorId.Equals(teachingScheduleData.TutorId));

        if (tutor.Data is null)
        {
            return RedirectToPage("/Error");
        }

        TutorDetail = _mapper.Map<TutorDto>((Data.Entities.Tutor)tutor.Data!);
        return Page();
    }
}
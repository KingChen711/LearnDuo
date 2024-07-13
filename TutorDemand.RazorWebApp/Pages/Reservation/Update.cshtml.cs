using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Enums;

namespace TutorDemand.RazorWebApp.Pages.Reservation;

public class UpdateModel : PageModel
{
    private readonly IReservationBusiness _reservationBusiness;
    private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;
    private readonly ISubjectBusiness _subjectBusiness;
    private readonly IMapper _mapper;

    public UpdateModel(IReservationBusiness reservationBusiness,
        ITeachingScheduleBusiness teachingScheduleBusiness,
        ISubjectBusiness subjectBusiness,
        IMapper mapper)
    {
        _reservationBusiness = reservationBusiness;
        _teachingScheduleBusiness = teachingScheduleBusiness;
        _subjectBusiness = subjectBusiness;
        _mapper = mapper;
    }
        
    public void OnGet()
    {
        
    }

    [BindProperty] public ReservationUpdateDto UpdateRequest { get; set; } = null!;


    public async Task<IActionResult> OnPost(Guid id)
    {
        var reservations = await _reservationBusiness.GetAllAsync();
        var reseravtionData = (List<Data.Entities.Reservation>)reservations.Data!;
        var updatedReservation = reseravtionData.FirstOrDefault(r => r.ReservationId.Equals(id));
        if (updatedReservation is null)
        {
            return NotFound();
        }

        updatedReservation.ReservationStatus = UpdateRequest.ReservationStatus;
        updatedReservation.PaymentMethod = UpdateRequest.PaymentMethod;

        if (!updatedReservation.PaymentStatus.Equals(PaymentStatus.Paid.ToString()))
        {
            updatedReservation.PaymentStatus = UpdateRequest.PaymentStatus;
        }

        var result = await _reservationBusiness.UpdateAsync(updatedReservation!);
        if (result.Status == 1)
        {
            var teachingScheduleResult = await _teachingScheduleBusiness.GetWithConditionAsync(x =>
                x.TeachingScheduleId.Equals(updatedReservation.TeachingScheduleId), null!, "Subject");
            var teachingList = _mapper.Map<List<TeachingSchedule>>(teachingScheduleResult.Data);
            var teachingSchedule = teachingList.FirstOrDefault();

            if (teachingSchedule != null
                // Check if change payment status from unpaid -> paid
                && UpdateRequest.PaymentStatus.Equals(PaymentStatus.Paid.ToString()))
            {
                // Increase enrolled student in subject
                teachingSchedule.Subject.EnrolledStudents += 1;
                // Save to DB
                await _subjectBusiness.UpdateAsync(_mapper.Map<SubjectDto>(teachingSchedule.Subject));
            }

            return RedirectToPage("/Reservation/Index");
        }
        else
        {
            return RedirectToPage("/Error");
        }
    }

}
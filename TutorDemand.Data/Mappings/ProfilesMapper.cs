using AutoMapper;
using System.Linq.Expressions;
using TutorDemand.Data.Dtos.Customer;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Dtos.Slot;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Mappings
{
    public class ProfilesMapper : Profile
    {
        public ProfilesMapper()
        {
            CreateMap<Subject, SubjectDto>().ReverseMap();
            CreateMap<SubjectDto, SubjectAddDto>().ReverseMap();
            CreateMap<SubjectDto, SubjectUpdateDto>().ReverseMap();
            CreateMap<Tutor, TutorDto>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<TeachingSchedule, TeachingScheduleDto>().ReverseMap();
            CreateMap<Reservation, ReservationDetails>().ReverseMap();
            CreateMap<Reservation, ReservationCreateDto>().ReverseMap();
            CreateMap<Slot, SlotDto>().ReverseMap();

            // Special mapping 
            CreateMap<Expression<Func<Subject, bool>>, Expression<Func<SubjectDto, bool>>>().ReverseMap();
        }
    }
}
using AutoMapper;
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
            CreateMap<Tutor, TutorDto>().ReverseMap();
            CreateMap<TeachingSchedule, TeachingScheduleDto>().ReverseMap();
        }
    }
}

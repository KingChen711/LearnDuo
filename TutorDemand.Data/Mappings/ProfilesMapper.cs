using AutoMapper;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Mappings
{
    public class ProfilesMapper : Profile
    {
        public ProfilesMapper()
        {
            CreateMap<Subject, SubjectDto>().ReverseMap();
            CreateMap<SubjectDto, SubjectAddDto>().ReverseMap();
        }
    }
}

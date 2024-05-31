using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.RazorWebApp.Pages.Subject
{
    public class ViewModel : PageModel
    {
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly IMapper _mapper;
        private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;

        public ViewModel(ISubjectBusiness subjectBusiness,
            IMapper mapper,
            ITeachingScheduleBusiness teachingScheduleBusiness)
        {
            _subjectBusiness = subjectBusiness;
            _mapper = mapper;
            _teachingScheduleBusiness = teachingScheduleBusiness;
        }

        
        public SubjectDto Subject { get; set; } = null!;
        public List<TutorDto> Tutors { get; set; } = new List<TutorDto>();
        // Related subjects
        public List<SubjectDto> OtherSubjects { get; set; }

        public async Task OnGetAsync(Guid subjectId)
        {
            var businessResult = await _subjectBusiness.GetWithConditionAysnc(x => 
                // Building query to access data
                x.SubjectId.ToString().ToUpper().Equals(subjectId.ToString().ToUpper()),
                null!, // Without filter
                "TeachingSchedules"); // Include teaching schedule 

            if(businessResult.Status == Const.SUCCESS_READ_CODE)
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
                businessResult = await _teachingScheduleBusiness.GetWithConditionAysnc(x =>
                    // Get all teaching schedule 
                    teachingScheduleIds.Contains(x.Id), 
                    null!, // Without filter
                    "Tutor"); // Include tutor 

                if(businessResult.Status == Const.SUCCESS_READ_CODE)
                {
                    var teachingSchedules = _mapper.Map<List<TeachingScheduleDto>>(businessResult.Data);
                    
                    foreach(var ts in teachingSchedules)
                    {
                        Tutors.Add(ts.Tutor);
                    }
                }
            }
        }
    }
}

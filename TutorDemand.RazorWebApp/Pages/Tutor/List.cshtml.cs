using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Utils;
using TutorDemand.RazorWebApp.Models;

namespace TutorDemand.RazorWebApp.Pages.Tutor
{
    public class ListModel : PageModel
    {
        private readonly ITutorBusiness _tutorBusiness;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ListModel(ITutorBusiness tutorBusiness, IMapper mapper, IOptionsMonitor<AppSettings> monitor)
        {
            _tutorBusiness = tutorBusiness;
            _mapper = mapper;
            _appSettings = monitor.CurrentValue;
        }

        [BindProperty] public PaginatedList<TutorDto> Tutors { get; set; }

        public async Task OnGet(int? pageIndex = 1)
        {
            IBusinessResult businessResult = await _tutorBusiness.GetAllAsync();

            if (businessResult != null && businessResult.Status == 1)
            {
                var tutorList = _mapper.Map<List<TutorDto>>(businessResult.Data);

                // Paging
                pageIndex ??= 1;
                Tutors = PaginatedList<TutorDto>.Paging(tutorList, pageIndex.Value, _appSettings.PageSize);
            }
        }
    }
}
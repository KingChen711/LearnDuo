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

        public async Task OnGet(int? pageIndex = 1, string? name = null, string? email = null, string? phone = null, string? address = null)
        {
            IBusinessResult businessResult = await _tutorBusiness.GetAllAsync();

            if (businessResult != null && businessResult.Status == 1)
            {
                var tutorList = _mapper.Map<List<TutorDto>>(businessResult.Data);
                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(phone) ||
                    !string.IsNullOrEmpty(address))
                {
                    var filteredTutors = new List<TutorDto>();

                    foreach (var tutor in tutorList)
                    {
                        // Check if any of the criteria match
                        bool matchName = !string.IsNullOrEmpty(name) && tutor.Fullname.Contains(name, StringComparison.OrdinalIgnoreCase);
                        bool matchEmail = !string.IsNullOrEmpty(email) && tutor.Email.Contains(email, StringComparison.OrdinalIgnoreCase);
                        bool matchPhone = !string.IsNullOrEmpty(phone) && tutor.Phone.Contains(phone, StringComparison.OrdinalIgnoreCase);
                        bool matchAddress = !string.IsNullOrEmpty(address) && tutor.Address.Contains(address, StringComparison.OrdinalIgnoreCase);

                        if (matchName || matchEmail || matchPhone || matchAddress)
                        {
                            filteredTutors.Add(tutor);
                        }
                    }

                    // Paging
                    pageIndex ??= 1;
                    Tutors = PaginatedList<TutorDto>.Paging(filteredTutors, pageIndex.Value, 5);
                }
                else
                {
                    // Paging
                    pageIndex ??= 1;
                    Tutors = PaginatedList<TutorDto>.Paging(tutorList, pageIndex.Value, 5);
                }
            }
        }
    }
}
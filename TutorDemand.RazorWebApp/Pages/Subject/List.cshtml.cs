using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.Linq;
using System.Text.Json;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Utils;
using TutorDemand.RazorWebApp.Models;
using TutorDemand.RazorWebApp.Pages.Subject.Models;

namespace TutorDemand.RazorWebApp.Pages.Subject
{
    public class ListModel : PageModel
    {
        // Bind Properties
        [BindProperty]
        public List<SubjectDto> Subjects { get; set; } = null!;
        

        //[BindProperty]
        //public int PageIndex { get; set; } = 1;
        //[BindProperty]
        //public int TotalPage { get; set; }

        private readonly ISubjectBusiness _subjectBusiness;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ListModel(ISubjectBusiness subjectBusiness,
            IMapper mapper,
            IOptionsMonitor<AppSettings> monitor)
        {
            _subjectBusiness = subjectBusiness;
            _mapper = mapper;
            _appSettings = monitor.CurrentValue;
        }

        public async Task OnGet(int? pageIndex = 1)
        {
            IBusinessResult businessResult = null!;

            businessResult = await _subjectBusiness.GetAllAsync();
            Subjects = _mapper.Map<List<SubjectDto>>(businessResult.Data);

            // Paging
            var pagination =
                PaginatedList<SubjectDto>.Paging(Subjects, pageIndex ??= 1,
                    _appSettings.PageSize); // Default 7 record each page
            Subjects = pagination;
            TempData["PageIndex"] = pageIndex ??= 1;
            TempData["TotalPage"] = pagination.TotalPage;
        }

        public IActionResult OnPostFilter([FromBody] SubjectPostRequest reqObj)
        {
            IBusinessResult businessResult = null!;

            // Process search subject...
            var toLowerSearchTerm = reqObj.SearchValue.ToLower();
            businessResult = _subjectBusiness.GetWithCondition(x =>
                x.Name.ToLower().Contains(toLowerSearchTerm)
                || x.SubjectCode.ToLower().Contains(toLowerSearchTerm));

            // Access data from business data (if any) and map to List<SubjectDto>
            Subjects = _mapper.Map<List<SubjectDto>>(businessResult.Data);

            // Process filter subject...
            if (!String.IsNullOrEmpty(reqObj.OrderBy))
            {
                var isDescendingOrder = reqObj.OrderBy.StartsWith("-");

                Subjects = isDescendingOrder
                    ? Subjects.OrderByDescending(x => x.Name).ToList()
                    : Subjects.OrderBy(x => x.Name).ToList();
            }

            // Pagination
            var pagination = PaginatedList<SubjectDto>.Paging(Subjects, reqObj.PageIndex, _appSettings.PageSize);
            Subjects = pagination;
            TempData["TotalPage"] = pagination.TotalPage;
            TempData["PageIndex"] = reqObj.PageIndex;

            return new JsonResult(new
                { PageIndex = pagination.PageIndex, TotalPage = pagination.TotalPage, Subjects = Subjects });
            //return new JsonResult(Subjects);
        }
    }
}
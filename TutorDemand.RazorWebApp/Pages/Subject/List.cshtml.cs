using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Buffers;
using System.Linq;
using System.Linq.Expressions;
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

        [BindProperty]
        public SubjectPostRequest SubjectFilter { get; set; } = new SubjectPostRequest();

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

        public async Task<IActionResult> OnPostFormFilterAsync()
        {
            IBusinessResult businessResult = null!;

            if (!ModelState.IsValid)
            {
                businessResult = await _subjectBusiness.GetAllAsync();
                return Page();
            }

            // Check if request from filter form
            if (!String.IsNullOrEmpty(SubjectFilter.SubjectName?.ToString())
                || !String.IsNullOrEmpty(SubjectFilter.SubjectCode?.ToString())
                || !String.IsNullOrEmpty(SubjectFilter.StartDate.ToString())
                || !String.IsNullOrEmpty(SubjectFilter.EndDate.ToString())
                || !String.IsNullOrEmpty(SubjectFilter.Price.ToString()))
            {

                DateTime.TryParse(SubjectFilter.StartDate.ToString(), out var validStartDate);
                DateTime.TryParse(SubjectFilter.EndDate.ToString(), out var validEndDate);
                Decimal.TryParse(SubjectFilter.Price.ToString()?.Replace('.', ' ').Replace(" ", string.Empty), out var validPrice);


                businessResult = await _subjectBusiness.GetWithConditionAysnc(x =>
                    (string.IsNullOrEmpty(SubjectFilter.SubjectCode) || x.SubjectCode.Contains(SubjectFilter.SubjectCode)) &&
                    (string.IsNullOrEmpty(SubjectFilter.SubjectName) || x.Name.Contains(SubjectFilter.SubjectName)) &&
                    (!SubjectFilter.StartDate.HasValue || (x.StartDate.HasValue && x.StartDate.Value.Date >= validStartDate.Date)) &&
                    (!SubjectFilter.EndDate.HasValue || (x.EndDate.HasValue && x.EndDate.Value.Date <= validEndDate.Date)) &&
                    (!SubjectFilter.Price.HasValue || x.CostPrice == validPrice));
            }
            else
            {
                businessResult = await _subjectBusiness.GetAllAsync();
            }


            Subjects = _mapper.Map<List<SubjectDto>>(businessResult.Data);

            // Paging
            var pagingList = PaginatedList<SubjectDto>.Paging(Subjects, 1,
                    _appSettings.PageSize);
            Subjects = pagingList;
            TempData["PageIndex"] = Subjects.Any() ? 1 : 0;
            TempData["TotalPage"] = pagingList.TotalPage;

            return Page();
        }

        public IActionResult OnPostFilter([FromBody] SubjectPostRequest reqObj)
        {
            IBusinessResult businessResult = null!;

            // Process search term subject...
            var toLowerSearchTerm = reqObj.SearchValue.ToLower();
            businessResult = _subjectBusiness.GetWithCondition(x =>
                x.Name.ToLower().Contains(toLowerSearchTerm)
                || x.SubjectCode.ToLower().Contains(toLowerSearchTerm));

            // Access data from business data (if any) and map to List<SubjectDto>
            Subjects = _mapper.Map<List<SubjectDto>>(businessResult.Data);

            // Process filter subject...
            if (!String.IsNullOrEmpty(reqObj.OrderBy))
            {
                Subjects = SortingHelper.SortSubjectsByColumn(Subjects, reqObj.OrderBy).ToList();
            }

            // Pagination
            var pagination = PaginatedList<SubjectDto>.Paging(Subjects, reqObj.PageIndex, _appSettings.PageSize);
            Subjects = pagination;
            TempData["TotalPage"] = pagination.TotalPage;
            TempData["PageIndex"] = reqObj.PageIndex;

            if (reqObj is not null) return new JsonResult(new
            { PageIndex = pagination.PageIndex, TotalPage = pagination.TotalPage, Subjects = Subjects });

            return Page();
        }
    }
}
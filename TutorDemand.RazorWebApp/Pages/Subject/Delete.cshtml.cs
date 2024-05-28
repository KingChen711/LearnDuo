using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Utils;

namespace TutorDemand.RazorWebApp.Pages.Subject
{
    public class DeleteModel : PageModel
    {
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly IMapper _mapper;

        public DeleteModel(ISubjectBusiness subjectBusiness,
            IMapper mapper)
        {
            _subjectBusiness = subjectBusiness;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGet(Guid subjectId, int pageIndex)
        {
            // Process delete subject...
            IBusinessResult result = await _subjectBusiness.DeleteAsync(subjectId);

            // Check if success
            if (result.Status == Const.SUCCESS_DELETE_CODE)
            {
                // Get all subjects
                result = await _subjectBusiness.GetAllAsync();

                var pagination = PaginatedList<SubjectDto>.Paging(_mapper.Map<List<SubjectDto>>(result.Data), pageIndex, 7); // Default 7 record each page

                return new JsonResult(new { PageIndex = pagination.PageIndex, TotalPage = pagination.TotalPage, Subjects = pagination.ToList() });
            }

            return RedirectToAction("/Subject/List");
        }
    }
}

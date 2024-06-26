using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Customer;

namespace TutorDemand.RazorWebApp.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly ICustomerBusiness _customerBusiness;
        private readonly IMapper _mapper;

        public EditModel(ICustomerBusiness customerBusiness, IMapper mapper)
        {
            _customerBusiness = customerBusiness;
            _mapper = mapper;
        }

        [BindProperty] public CustomerDto Customer { get; set; }
        [BindProperty] public string idEdit { get; set; }

        public async Task OnGet(Guid id)
        {
            IBusinessResult businessResult = await _customerBusiness.FindOneAsync(x => x.CustomerId.Equals(id));
            if (businessResult != null)
            {
                Customer = _mapper.Map<CustomerDto>(businessResult.Data);
            }
        }

        public async Task<IActionResult> OnPost()
        {
            Customer.CustomerId = Guid.Parse(idEdit);
            await _customerBusiness.UpdateAsync(Customer);
            return RedirectToPage("/customers/list");
        }
    }
}

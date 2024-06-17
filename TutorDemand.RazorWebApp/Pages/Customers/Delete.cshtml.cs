using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;

namespace TutorDemand.RazorWebApp.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        private readonly ICustomerBusiness _customerBusiness;

        public DeleteModel(ICustomerBusiness customerBusiness)
        {
            _customerBusiness = customerBusiness;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            IBusinessResult businessResult = await _customerBusiness.FindOneAsync(x => x.CustomerId.Equals(id));
            if (businessResult != null)
            {
                await _customerBusiness.DeleteAsync(id);
                return RedirectToPage("/customers/list");
            }
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.Customer;


namespace TutorDemand.RazorWebApp.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerBusiness _customerBusiness;

        public CreateModel(ICustomerBusiness customerBusiness)
        {
            this._customerBusiness = customerBusiness;
        }

        [BindProperty] public CustomerDto  Customer { get; set; }

        public async Task<IActionResult> OnPost()
        {
            Customer.CustomerId = Guid.NewGuid();
            Customer.Password = "123456";
            await _customerBusiness.CreateAsync(Customer);
            return RedirectToPage("/customers/list");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Entities;
using TutorDemand.RazorWebApp.Models;


namespace TutorDemand.RazorWebApp.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly ICustomerBusiness _customerBusiness;

        public LoginModel(ICustomerBusiness customerBusiness)
        {
            _customerBusiness = customerBusiness;
        }

        [BindProperty]
        public string email { get; set; }
        
        [BindProperty]
        public string password { get; set; }

        [BindProperty]
        public string ExistedCustomer { get; set; }

        public IActionResult OnGet()
        {
            // Check if customer is already logged in
            var existingCustomer = SessionHelpers.GetObjectFromJson<Customer>(HttpContext.Session,"Customer");
            if (existingCustomer != null)
            {
                ExistedCustomer = existingCustomer.Email;
                // Redirect to index page or show message that customer is already logged in
                ViewData["ErrorMessage"] = "Customer is already logged in.";
            }
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostLogin()
        {
            var cutomer = await _customerBusiness.FindOneAsync(x => x.Email.Equals(email) && x.Password.Equals(password));
            if (cutomer.Status == 1)
            {
                // Store user data in session
                SessionHelpers.SetObjectAsJson(HttpContext.Session,"Customer", cutomer.Data);
                return RedirectToPage("/Index");
            }
            else 
            {
                ViewData["ErrorMessage"] = "Invalid email or password !!!";
                return Page();
            }
        }
    }
}


using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Customer;
using TutorDemand.Data.Utils;
using TutorDemand.RazorWebApp.Models;

namespace TutorDemand.RazorWebApp.Pages.Customers
{
    public class ListModel : PageModel
    {
     private readonly ICustomerBusiness _customerBusiness;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ListModel(ICustomerBusiness customerBusiness, IMapper mapper, IOptionsMonitor<AppSettings> monitor)
        {
            _customerBusiness = customerBusiness;
            _mapper = mapper;
            _appSettings = monitor.CurrentValue;
        }

        [BindProperty] public PaginatedList<CustomerDto> Customers { get; set; }

        public async Task OnGet(int? pageIndex = 1, string? name = null, string? email = null, string? phone = null, string? address = null)
        {
            IBusinessResult businessResult = await _customerBusiness.GetAllAsync();

            if (businessResult != null && businessResult.Status == 1)
            {
                var customerList = _mapper.Map<List<CustomerDto>>(businessResult.Data);
                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(phone) ||
                    !string.IsNullOrEmpty(address))
                {
                    var filteredCustomers = new List<CustomerDto>();

                    foreach (var customer in customerList)
                    {
                        // Check if any of the criteria match
                        bool matchName = !string.IsNullOrEmpty(name) && customer.Fullname.Contains(name, StringComparison.OrdinalIgnoreCase);
                        bool matchEmail = !string.IsNullOrEmpty(email) && customer.Email.Contains(email, StringComparison.OrdinalIgnoreCase);
                        bool matchPhone = !string.IsNullOrEmpty(phone) && customer.Phone.Contains(phone, StringComparison.OrdinalIgnoreCase);
                        bool matchAddress = !string.IsNullOrEmpty(address) && customer.Address.Contains(address, StringComparison.OrdinalIgnoreCase);

                        if (matchName || matchEmail || matchPhone || matchAddress)
                        {
                            filteredCustomers.Add(customer);
                        }
                    }

                    // Paging
                    pageIndex ??= 1;
                    Customers = PaginatedList<CustomerDto>.Paging(filteredCustomers, pageIndex.Value, 5);
                }
                else
                {
                    // Paging
                    pageIndex ??= 1;
                    Customers = PaginatedList<CustomerDto>.Paging(customerList, pageIndex.Value, 5);
                }
            }
        }
    }
}

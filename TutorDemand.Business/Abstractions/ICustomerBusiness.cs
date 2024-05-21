using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Dtos.Customer;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions
{
    public interface ICustomerBusiness
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<IEnumerable<Customer>> Find(Expression<Func<Customer, bool>> expression);
        Task<Customer?> FindOne(Expression<Func<Customer, bool>> expression);
        Task Create(CustomerAddDto entity);
        Task Update(CustomerUpdateDto entity);
        Task Delete(Guid customerId);
    }
}

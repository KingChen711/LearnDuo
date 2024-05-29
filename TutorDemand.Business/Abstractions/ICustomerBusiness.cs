using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Customer;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions
{
    public interface ICustomerBusiness
    {
        Task<IBusinessResult> GetAllAsync();
        Task<IBusinessResult> FindOneAsync(Expression<Func<Customer, bool>> expression);
        Task<IBusinessResult> CreateAsync(CustomerAddDto entity);
        Task<IBusinessResult> UpdateAsync(CustomerUpdateDto entity);
        Task<IBusinessResult> DeleteAsync(Guid tutorId);
    }
}
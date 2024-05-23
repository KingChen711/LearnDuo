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
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> Find(Expression<Func<Customer, bool>> filter = null!,
            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = null!,
            string includeProperties = "");
        Task<IBusinessResult> FindOne(Expression<Func<Customer, bool>> expression);
        Task<IBusinessResult> Create(CustomerAddDto entity);
        Task<IBusinessResult> Update(CustomerUpdateDto entity);
        Task<IBusinessResult> Delete(Guid tutorId);
    }
}

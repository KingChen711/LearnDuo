using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.Customer;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly NET1704_221_5_TutorDemandContext tutorDemandContext;

        public CustomerBusiness(NET1704_221_5_TutorDemandContext tutorDemandContext)
        {
            this.tutorDemandContext = tutorDemandContext;
        }
        public async Task<IEnumerable<Customer>> GetAll() => await tutorDemandContext.Customers.ToListAsync();
        public async Task<IEnumerable<Customer>> Find(Expression<Func<Customer, bool>> expression) => await tutorDemandContext.Customers.Where(expression).ToListAsync();
        public async Task<Customer?> FindOne(Expression<Func<Customer, bool>> expression) => await tutorDemandContext.Customers.Where(expression).FirstOrDefaultAsync();
        public async Task Create(CustomerAddDto dto)
        {
            var entity = dto.Adapt<Customer>();
            tutorDemandContext.Customers.Add(entity);
            await tutorDemandContext.SaveChangesAsync();
        }
        public async Task Delete(Guid customerId)
        {
            var entity = await FindOne(t => t.CustomerId == customerId);

            if (entity is null)
            {
                throw new Exception($"Not found Customer with id: ${customerId}");
            }

            tutorDemandContext.Customers.Remove(entity);
            await tutorDemandContext.SaveChangesAsync();
        }
        public async Task Update(CustomerUpdateDto dto)
        {
            var entity = dto.Adapt<Customer>();
            tutorDemandContext.Customers.Update(entity);
            await tutorDemandContext.SaveChangesAsync();
        }
    }
}

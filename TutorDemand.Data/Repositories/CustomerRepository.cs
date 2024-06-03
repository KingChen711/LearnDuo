using TutorDemand.Data.Base;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Repositories;

public class CustomerRepository : GenericRepository<Customer>
{
    private readonly NET1704_221_5_TutorDemandContext _context;
    public CustomerRepository()
    {

    }

    public CustomerRepository(NET1704_221_5_TutorDemandContext context)
    {
        _context = context;
    }
}
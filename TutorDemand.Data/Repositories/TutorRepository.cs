using TutorDemand.Data.Base;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Repositories;

public class TutorRepository : GenericRepository<Tutor>
{
    public TutorRepository()
    {

    }

    public TutorRepository(NET1704_221_5_TutorDemandContext context)
    {
        _context = context;
    }
}
using TutorDemand.Data.Base;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Repositories;

public class SlotRepository: GenericRepository<Slot>
{
    private readonly NET1704_221_5_TutorDemandContext _context;
    public SlotRepository()
    {

    }

    public SlotRepository(NET1704_221_5_TutorDemandContext context)
    {
        _context = context;
    }
}
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business
{
    public class TutorBusiness : ITutorBusiness
    {
        private readonly NET1704_221_5_TutorDemandContext tutorDemandContext;

        public TutorBusiness(NET1704_221_5_TutorDemandContext tutorDemandContext)
        {
            this.tutorDemandContext = tutorDemandContext;
        }
        public async Task<IEnumerable<Tutor>> GetAll() => await tutorDemandContext.Tutors.ToListAsync();
        public async Task<IEnumerable<Tutor>> Find(Expression<Func<Tutor, bool>> expression) => await tutorDemandContext.Tutors.Where(expression).ToListAsync();
        public async Task<Tutor?> FindOne(Expression<Func<Tutor, bool>> expression) => await tutorDemandContext.Tutors.Where(expression).FirstOrDefaultAsync();
        public async Task Create(TutorAddDto dto)
        {
            var entity = dto.Adapt<Tutor>();
            tutorDemandContext.Tutors.Add(entity);
            await tutorDemandContext.SaveChangesAsync();
        }
        public async Task Delete(Guid tutorId)
        {
            var entity = await FindOne(t => t.TutorId == tutorId);

            if (entity is null)
            {
                throw new Exception($"Not found Tutor with id: ${tutorId}");
            }

            tutorDemandContext.Tutors.Remove(entity);
            await tutorDemandContext.SaveChangesAsync();
        }
        public async Task Update(TutorUpdateDto dto)
        {
            var entity = dto.Adapt<Tutor>();
            tutorDemandContext.Tutors.Update(entity);
            await tutorDemandContext.SaveChangesAsync();
        }
    }
};


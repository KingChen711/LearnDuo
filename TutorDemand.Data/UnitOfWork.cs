using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Repositories;

namespace TutorDemand.Data
{
    public class UnitOfWork
    {
        private NET1704_221_5_TutorDemandContext _context;
        private SubjectRepository _subjectRepo;
        private TeachingScheduleRepository _teachingScheduleRepo;
        private TutorRepository _tutorRepository;
        private CustomerRepository _customerRepository;
        private ReservationRepository _reservationRepository;
        private SlotRepository _slotRepository;

        public UnitOfWork()
        {
            _context ??= new NET1704_221_5_TutorDemandContext();
        }

        public SubjectRepository SubjectRepository
        {
            get { return _subjectRepo ??= new SubjectRepository(); }
        }

        public TeachingScheduleRepository TeachingScheduleRepository
        {
            get { return _teachingScheduleRepo ??= new TeachingScheduleRepository(); }
        }

        public TutorRepository TutorRepository
        {
            get { return _tutorRepository ??= new TutorRepository(); }
        }

        public ReservationRepository ReservationRepository
        {
            get
            {
                return _reservationRepository ??= new ReservationRepository();
            }
        }
        public CustomerRepository CustomerRepository
        {
            get { return _customerRepository ??= new CustomerRepository(); }
        }

        public SlotRepository SlotRepository
        {
            get { return _slotRepository ??= new SlotRepository(); }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

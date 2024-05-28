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
        private ReservationRepository _reservationRepository;

        public UnitOfWork()
        {
        }

        public SubjectRepository SubjectRepository
        {
            get
            {
                return _subjectRepo ??= new SubjectRepository();
            }
        }

        public ReservationRepository ReservationRepository
        {
            get
            {
                return _reservationRepository ??= new ReservationRepository();
            }
        }
    }
}

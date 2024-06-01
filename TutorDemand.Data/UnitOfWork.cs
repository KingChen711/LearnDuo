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
        private NET1704_221_5_TutorDemandContext _unitOfWorkContext;
        private SubjectRepository _subjectRepo;
        private TeachingScheduleRepository _teachingScheduleRepo;
        private TutorRepository _tutorRepository;
        private CustomerRepository _customerRepository;
        private SlotRepository _slotRepository;

        public UnitOfWork()
        {
            _context ??= new NET1704_221_5_TutorDemandContext();
        }

        public UnitOfWork(NET1704_221_5_TutorDemandContext context) => _unitOfWorkContext = context;


        public SubjectRepository SubjectRepository
        {
            get { return _subjectRepo ??= new SubjectRepository(_unitOfWorkContext); }
        }

        public TeachingScheduleRepository TeachingScheduleRepository
        {
            get { return _teachingScheduleRepo ??= new TeachingScheduleRepository(_unitOfWorkContext); }
        }

        public TutorRepository TutorRepository
        {
            get { return _tutorRepository ??= new TutorRepository(_unitOfWorkContext); }
        }

        public CustomerRepository CustomerRepository
        {
            get { return _customerRepository ??= new CustomerRepository(_unitOfWorkContext); }
        }

        public SlotRepository SlotRepository
        {
            get { return _slotRepository ??= new SlotRepository(_unitOfWorkContext); }
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWorkContext.SaveChangesAsync();
        }

        ////TO-DO CODE HERE/////////////////

        #region Set transaction isolation levels

        /*
        Read Uncommitted: The lowest level of isolation, allows transactions to read uncommitted data from other transactions. This can lead to dirty reads and other issues.

        Read Committed: Transactions can only read data that has been committed by other transactions. This level avoids dirty reads but can still experience other isolation problems.

        Repeatable Read: Transactions can only read data that was committed before their execution, and all reads are repeatable. This prevents dirty reads and non-repeatable reads, but may still experience phantom reads.

        Serializable: The highest level of isolation, ensuring that transactions are completely isolated from one another. This can lead to increased lock contention, potentially hurting performance.

        Snapshot: This isolation level uses row versioning to avoid locks, providing consistency without impeding concurrency. 
         */

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _unitOfWorkContext.Database.BeginTransaction())
            {
                try
                {
                    result = _unitOfWorkContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _unitOfWorkContext.Database.BeginTransaction())
            {
                try
                {
                    result = await _unitOfWorkContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        #endregion

    }
}

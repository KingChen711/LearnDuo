using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data.DAO;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business
{
    public class TutorBusiness : ITutorBusiness
    {
        private readonly TutorDAO tutorDAO;

        public TutorBusiness() 
        {
            this.tutorDAO = new TutorDAO();
        }
        public async Task<IBusinessResult> CreateAsync(TutorAddDto dto)
        {
            try
            {
                var entity = dto.Adapt<Tutor>();
                tutorDAO.Create(entity);
                var result = await tutorDAO.SaveChangesAsync() > 0;
                if (result)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }

        public async Task<IBusinessResult> DeleteAsync(Guid tutorId)
        {
            var tutorEntity = await tutorDAO.GetByIdAsync(tutorId);
            if (tutorEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                tutorDAO.Remove(tutorEntity);
                await tutorDAO.SaveChangesAsync();

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }

        public async Task<IBusinessResult> FindAsync(Expression<Func<Tutor, bool>> filter = null!,
        Func<IQueryable<Tutor>, IOrderedQueryable<Tutor>> orderBy = null!,
        string includeProperties = "")
        {
            try
            {
                var tutors = await tutorDAO.GetWithConditionAsync(filter, orderBy, includeProperties);

                if (tutors != null)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, tutors);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }

            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }

        public async Task<IBusinessResult> FindOneAsync(Expression<Func<Tutor, bool>> expression)
        {
            try
            {
                var tutors = await tutorDAO.FindOneAsync(expression);
                if (tutors != null)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, tutors);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAllAsync()
        {
            try
            {
                var tutorEntities = tutorDAO.GetAll();

                if (tutorEntities != null)
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, tutorEntities);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateAsync(TutorUpdateDto dto)
        {
            try
            {
                var entity = dto.Adapt<Tutor>();
                await tutorDAO.UpdateAsync(entity);
                var result = await tutorDAO.SaveChangesAsync() > 0;

                if (result)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }

            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }
    }
};


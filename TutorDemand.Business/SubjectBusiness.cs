using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data.DAO;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business;

public class SubjectBusiness : ISubjectBusiness
{
    private readonly SubjectDAO _subjectDAO;

    public SubjectBusiness()
    {
        _subjectDAO = new SubjectDAO();
    }

    public IBusinessResult Delete(Guid subjectId)
    {
        try
        {
            var subjectEntity = _subjectDAO.GetById(subjectId);
            if (subjectEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                _subjectDAO.Remove(subjectEntity);
                _subjectDAO.SaveChanges();

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }
        catch(Exception ex)
        {
            return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
        }
    }

    public async Task<IBusinessResult> DeleteAsync(Guid subjectId)
    {
        var subjectEntity = await _subjectDAO.GetByIdAsync(subjectId);
        if (subjectEntity is null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        }
        else
        {
            _subjectDAO.Remove(subjectEntity);
            await _subjectDAO.SaveChangesAsync();

            return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
        }
    }

    public IBusinessResult GetById(Guid subjectId)
    {
        try
        {
            var subjectEntity = _subjectDAO.GetById(subjectId);

            if(subjectEntity is null)
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, subjectEntity!);
            }
            else
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }catch(Exception ex)
        {
            return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
        }
    }

    public async Task<IBusinessResult> GetByIdAsync(Guid subjectId)
    {
        try
        {
            var subjectEntity = await _subjectDAO.GetByIdAsync(subjectId);

            if (subjectEntity is null)
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, subjectEntity!);
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

    public IBusinessResult GetAll()
    {
        try
        {
            var subjectEntities = _subjectDAO.GetAll();

            if (subjectEntities.Any())
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, subjectEntities!);
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

    public async Task<IBusinessResult> GetAllAsync()
    {
        try
        {
            var subjectEntities = await _subjectDAO.GetAllAsync();

            if (subjectEntities.Any())
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, subjectEntities!);
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


    public IBusinessResult GetWithCondition(
            Expression<Func<Subject, bool>> filter = null!, 
            Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!, 
            string includeProperties = "")
    {
        try
        {
            var subjects = _subjectDAO.GetWithCondition(filter, orderBy, includeProperties);
            var result = _subjectDAO.SaveChanges() > 0;

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

    public async Task<IBusinessResult> GetWithConditionAysnc(
        Expression<Func<Subject, bool>> filter = null!, 
        Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!, 
        string includeProperties = "")
    {
        try
        {
            var subjects = await _subjectDAO.GetWithConditionAsync(filter, orderBy, includeProperties);
            var result = await _subjectDAO.SaveChangesAsync() > 0;

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

    public IBusinessResult Insert(Subject subject)
    {
        try
        {
            _subjectDAO.Create(subject);
            var result = _subjectDAO.SaveChanges() > 0;
            if (result)
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
            }
            else
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
        }catch(Exception ex)
        {
            return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
        }
    }

    public async Task<IBusinessResult> InsertAsync(Subject subject)
    {
        try
        {
            await _subjectDAO.CreateAsync(subject);
            var result = await _subjectDAO.SaveChangesAsync() > 0;
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

    public IBusinessResult Update(Subject subject)
    {
        try
        {
            _subjectDAO.Update(subject);
            var result = _subjectDAO.SaveChanges() > 0;

            if (result)
            {
                return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            else
            {
                return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }

        }catch(Exception ex)
        {
            return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
        }
    }

    public async Task<IBusinessResult> UpdateAsync(Subject subject)
    {
        try
        {
            await _subjectDAO.UpdateAsync(subject);
            var result = await _subjectDAO.SaveChangesAsync() > 0;

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
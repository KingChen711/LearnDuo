using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.DAO;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business;

public class SubjectBusiness : ISubjectBusiness
{
    //private readonly SubjectDAO _unitOfWork.SubjectRepository;
    private readonly UnitOfWork _unitOfWork;

    public SubjectBusiness()
    {
        //_unitOfWork.SubjectRepository = new SubjectDAO();
        _unitOfWork ??= new UnitOfWork();
    }

    public IBusinessResult Delete(Guid subjectId)
    {
        try
        {
            var subjectEntity = _unitOfWork.SubjectRepository.GetById(subjectId);
            if (subjectEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                _unitOfWork.SubjectRepository.PrepareRemove(subjectEntity);
                _unitOfWork.SubjectRepository.Save();

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }
        catch(Exception ex)
        {
            return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
        }
    }

    public IBusinessResult Delete(int id)
    {
        try
        {
            var subjectEntity = _unitOfWork.SubjectRepository.GetById(id);
            if (subjectEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                _unitOfWork.SubjectRepository.PrepareRemove(subjectEntity);
                _unitOfWork.SubjectRepository.Save();

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }
        catch (Exception ex)
        {
            return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
        }
    }

    public async Task<IBusinessResult> DeleteAsync(Guid subjectId)
    {
        var subjectEntity = await _unitOfWork.SubjectRepository.GetByIdAsync(subjectId);
        if (subjectEntity is null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        }
        else
        {
            _unitOfWork.SubjectRepository.PrepareRemove(subjectEntity);
            await _unitOfWork.SubjectRepository.SaveAsync();

            return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
        }
    }

    public IBusinessResult GetById(Guid subjectId)
    {
        try
        {
            var subjectEntity = _unitOfWork.SubjectRepository.GetById(subjectId);

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
            var subjectEntity = await _unitOfWork.SubjectRepository.GetByIdAsync(subjectId);

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
            var subjectEntities = _unitOfWork.SubjectRepository.GetAll();

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
            var subjectEntities = await _unitOfWork.SubjectRepository.GetAllAsync();

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


    //public IBusinessResult GetWithCondition(
    //        Expression<Func<Subject, bool>> filter = null!, 
    //        Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!, 
    //        string includeProperties = "")
    //{
    //    try
    //    {
    //        var subjects = _unitOfWork.SubjectRepository.GetWithCondition(filter, orderBy, includeProperties);
    //        var result = _unitOfWork.SubjectRepository.Save() > 0;

    //        if (result)
    //        {
    //            return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
    //        }
    //        else
    //        {
    //            return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //    }
    //}

    //public async Task<IBusinessResult> GetWithConditionAysnc(
    //    Expression<Func<Subject, bool>> filter = null!, 
    //    Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!, 
    //    string includeProperties = "")
    //{
    //    try
    //    {
    //        var subjects = await _unitOfWork.SubjectRepository.GetWithConditionAsync(filter, orderBy, includeProperties);
    //        var result = await _unitOfWork.SubjectRepository.SaveAsync() > 0;

    //        if (result)
    //        {
    //            return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
    //        }
    //        else
    //        {
    //            return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //    }
    //}

    public IBusinessResult Create(Subject subject)
    {
        try
        {
            _unitOfWork.SubjectRepository.PrepareCreate(subject);
            var result = _unitOfWork.SubjectRepository.Save() > 0;
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

    public async Task<IBusinessResult> CreateAsync(Subject subject)
    {
        try
        {
            await _unitOfWork.SubjectRepository.CreateAsync(subject);
            var result = await _unitOfWork.SubjectRepository.SaveAsync() > 0;
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
            _unitOfWork.SubjectRepository.PrepareUpdate(subject);
            var result = _unitOfWork.SubjectRepository.Save() > 0;

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
            _unitOfWork.SubjectRepository.PrepareUpdate(subject);
            var result = await _unitOfWork.SubjectRepository.SaveAsync() > 0;

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

    public IBusinessResult GetById(int id)
    {
        try
        {
            var subjectEntity = _unitOfWork.SubjectRepository.GetById(id);

            if (subjectEntity is not null)
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

    public int SaveChanges()
    {
        return _unitOfWork.SubjectRepository.Save();
    }

    public async Task<int> SaveChangeAsync()
    {
        return await _unitOfWork.SubjectRepository.SaveAsync();
    }
}
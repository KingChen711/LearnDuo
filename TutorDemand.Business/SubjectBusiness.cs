using AutoMapper;
using System.Linq.Expressions;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business;

public class SubjectBusiness : ISubjectBusiness
{
    //private readonly SubjectDAO _unitOfWork.SubjectRepository;
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubjectBusiness(IMapper mapper, NET1704_221_5_TutorDemandContext context)
    {
        //_unitOfWork.SubjectRepository = new SubjectDAO();
        _unitOfWork ??= new UnitOfWork(context);
        _mapper = mapper;
    }

    public SubjectBusiness(IMapper mapper) { _mapper = mapper;  _unitOfWork ??= new UnitOfWork(); }

    public SubjectBusiness() => _unitOfWork ??= new UnitOfWork();


    public IBusinessResult Delete(Guid subjectId)
    {
        try
        {
            // An Error occured: Cannot find entity by typeof(Guid) id, this not primary key -> GetByIdAsync run failed

            //var subjectEntity = _unitOfWork.SubjectRepository.GetById(subjectId);

            // -> Use custom function
            var subjectEntities = _unitOfWork.SubjectRepository.GetWithCondition(x =>
                x.SubjectId.Equals(subjectId));

            if (!subjectEntities.Any())
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                _unitOfWork.SubjectRepository.PrepareRemove(subjectEntities.First());
                _unitOfWork.SubjectRepository.Save();

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }
        catch (Exception ex)
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
        // An Error occured: Cannot find entity by typeof(Guid) id, this not primary key -> GetByIdAsync run failed

        //var subjectEntity = await _unitOfWork.SubjectRepository.GetByIdAsync(subjectId);

        // -> Use custom function
        var subjectEntities = await _unitOfWork.SubjectRepository.GetWithConditionAsync(x =>
            x.SubjectId.Equals(subjectId));
        if (!subjectEntities.Any())
        {
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        }
        else
        {
            _unitOfWork.SubjectRepository.PrepareRemove(subjectEntities.First());
            await _unitOfWork.SubjectRepository.SaveAsync();

            return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
        }
    }

    public IBusinessResult GetById(Guid subjectId)
    {
        try
        {
            var subjectEntity = _unitOfWork.SubjectRepository.GetById(subjectId);

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

    public async Task<IBusinessResult> GetByIdAsync(Guid subjectId)
    {
        try
        {
            // An Error occured: Cannot find entity by typeof(Guid) id, this not primary key -> GetByIdAsync run failed

            //var subjectEntity = await _unitOfWork.SubjectRepository.GetByIdAsync(subjectId);

            // -> Use custom function
            var subjectEntities = await _unitOfWork.SubjectRepository.GetWithConditionAsync(x =>
                x.SubjectId.Equals(subjectId));

            if (subjectEntities.Any())
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, subjectEntities.First());
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


    public IBusinessResult GetWithCondition(
        Expression<Func<Subject, bool>> filter = null!,
        Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!,
        string includeProperties = "")
    {
        try
        {
            var subjects = _unitOfWork.SubjectRepository.GetWithCondition(filter, orderBy, includeProperties);

            if (subjects.Any())
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, subjects);
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

    public async Task<IBusinessResult> GetWithConditionAysnc(
        Expression<Func<Subject, bool>> filter = null!,
        Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!,
        string includeProperties = "")
    {
        try
        {
            var subjects =
                await _unitOfWork.SubjectRepository.GetWithConditionAsync(filter, orderBy, includeProperties);

            if (subjects.Any())
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, subjects);
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

    public IBusinessResult Create(SubjectDto subjectDto)
    {
        try
        {
            _unitOfWork.SubjectRepository.PrepareCreate(_mapper.Map<Subject>(subjectDto));
            var result = _unitOfWork.SubjectRepository.Save() > 0;
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

    public async Task<IBusinessResult> CreateAsync(SubjectDto subjectDto)
    {
        try
        {
            _unitOfWork.SubjectRepository.PrepareCreate(_mapper.Map<Subject>(subjectDto));
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

    public IBusinessResult Update(SubjectDto subjectDto)
    {
        try
        {
            _unitOfWork.SubjectRepository.PrepareUpdate(_mapper.Map<Subject>(subjectDto));
            var result = _unitOfWork.SubjectRepository.Save() > 0;

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

    public async Task<IBusinessResult> UpdateAsync(SubjectDto subjectDto)
    {
        try
        {
            var subjectEntities = await _unitOfWork.SubjectRepository.GetWithConditionAsync(x =>
                x.SubjectId.Equals(subjectDto.SubjectId));

            if (!subjectEntities.Any())
            {
                return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }

            var subject = subjectEntities.First();

            _unitOfWork.SubjectRepository.PrepareUpdate(subject);

            // This just temporary update attach code... -> Fix later
            subject.SubjectCode = subjectDto.SubjectCode;
            subject.Name = subjectDto.Name;
            subject.Description = subjectDto.Description;
            subject.Image = subjectDto.Image;
            subject.Duration = subjectDto.Duration;

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
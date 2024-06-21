using AutoMapper;
using Mapster;
using System.Linq.Expressions;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business
{
    public class TutorBusiness : ITutorBusiness
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TutorBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public TutorBusiness(IMapper mapper)  { _mapper = mapper;  _unitOfWork ??= new UnitOfWork(); }

        public async Task<IBusinessResult> FindOneAsync(Expression<Func<Tutor, bool>> condition)
        {
            try
            {
                var tutors = await _unitOfWork.TutorRepository.GetOneWithConditionAsync(condition);

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

        public async Task<IBusinessResult> CreateAsync(TutorDto dto)
        {
            try
            {
                var entity = dto.Adapt<Tutor>();
                var result =  await _unitOfWork.TutorRepository.CreateAsync(entity) > 0;
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
            var tutorEntity = _unitOfWork.TutorRepository.GetOneWithCondition(x => x.TutorId.Equals(tutorId));
            if (tutorEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                await _unitOfWork.TutorRepository.RemoveAsync(tutorEntity);

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }

        public async Task<IBusinessResult> GetAllAsync()
        {
            try
            {
                var tutorEntities = await _unitOfWork.TutorRepository.GetAllAsync();

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

        public async Task<IBusinessResult> UpdateAsync(TutorDto dto)
        {
            try
            {
                var entity = await _unitOfWork.TutorRepository.GetOneWithConditionAsync(x => x.TutorId.Equals(dto.TutorId));

                if (entity == null)
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }

                entity.Fullname = dto.Fullname;
                entity.Address = dto.Address;
                entity.Email = dto.Email;
                entity.Phone = dto.Phone;
                entity.Gender = dto.Gender;
                entity.CertificateImage = dto.CertificateImage;
                entity.IdentityCard = dto.IdentityCard;
                entity.Avatar = dto.Avatar;

               var result = await _unitOfWork.TutorRepository.UpdateAsync(entity);

                return result > 0
                    ? new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG)
                    : new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }

    }
};
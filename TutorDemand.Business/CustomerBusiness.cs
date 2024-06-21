using Mapster;
using System.Linq.Expressions;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.Dtos.Customer;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public CustomerBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        
        
        public async Task<IBusinessResult> DeleteAsync(Guid customerId)
        {
            var customerEntity = _unitOfWork.CustomerRepository.GetOneWithCondition(x => x.CustomerId.Equals(customerId));
            if (customerEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                await _unitOfWork.CustomerRepository.RemoveAsync(customerEntity);

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }

        public async Task<IBusinessResult> FindOneAsync(Expression<Func<Customer, bool>> expression)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetOneWithConditionAsync(expression);

                if (customer != null)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, customer);
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
                var customerEntities = await _unitOfWork.CustomerRepository.GetAllAsync();

                if (customerEntities != null)
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, customerEntities);
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

        public async Task<IBusinessResult> UpdateAsync(CustomerDto dto)
        {
            try
            {
                var entity = await _unitOfWork.CustomerRepository.GetOneWithConditionAsync(x => x.CustomerId.Equals(dto.CustomerId));

                if (entity == null)
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }

                entity.Fullname = dto.Fullname;
                entity.Address = dto.Address;
                entity.Email = dto.Email;
                entity.Phone = dto.Phone;
                entity.Gender = dto.Gender;
                entity.Avatar = dto.Avatar;

                var result = await _unitOfWork.CustomerRepository.UpdateAsync(entity);

                return result > 0
                    ? new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG)
                    : new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }
        
        public async Task<IBusinessResult> CreateAsync(CustomerDto dto)
        {
            try
            {
                var entity = dto.Adapt<Customer>();
                var result =  await _unitOfWork.CustomerRepository.CreateAsync(entity) > 0;
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
    }
}
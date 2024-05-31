using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.DAO;
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

        public async Task<IBusinessResult> CreateAsync(CustomerAddDto dto)
        {
            try
            {
                var entity = dto.Adapt<Customer>();
                await _unitOfWork.CustomerRepository.CreateAsync(entity);
                var result = await _unitOfWork.CustomerRepository.SaveAsync() > 0;
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
            var customerEntity = await _unitOfWork.CustomerRepository.GetByIdAsync(tutorId);
            if (customerEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                await _unitOfWork.CustomerRepository.RemoveAsync(customerEntity);
                await _unitOfWork.CustomerRepository.SaveAsync();

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }

        public async Task<IBusinessResult> FindOneAsync(Expression<Func<Customer, bool>> expression)
        {
            try
            {
                var customers = await _unitOfWork.CustomerRepository.GetWithConditionAsync(expression);

                if (customers != null)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, customers);
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

        public async Task<IBusinessResult> UpdateAsync(CustomerUpdateDto dto)
        {
            try
            {
                var entity = dto.Adapt<Customer>();
                await _unitOfWork.CustomerRepository.UpdateAsync(entity);
                var result = await _unitOfWork.CustomerRepository.SaveAsync() > 0;

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
}
using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data.DAO;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business;

public class ReservationBusiness : IReservationBusiness
{
    private readonly ReservationDAO _reservationDAO;
    public ReservationBusiness()
    {
        _reservationDAO = new ReservationDAO();    
    }

    public IBusinessResult Delete(Guid reservationId)
    {
        try
        {
            var subjectEntity = _reservationDAO.GetById(reservationId);
            if (subjectEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                _reservationDAO.Remove(subjectEntity);
                _reservationDAO.SaveChanges();

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
            var subjectEntity = _reservationDAO.GetById(id);
            if (subjectEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                _reservationDAO.Remove(subjectEntity);
                _reservationDAO.SaveChanges();

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }
        catch (Exception ex)
        {
            return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
        }
    }

    public async Task<IBusinessResult> DeleteAsync(Guid reservationId)
    {
        var subjectEntity = await _reservationDAO.GetByIdAsync(reservationId);
        if (subjectEntity is null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        }
        else
        {
            _reservationDAO.Remove(subjectEntity);
            await _reservationDAO.SaveChangesAsync();

            return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
        }
    }

    public IBusinessResult GetById(Guid reservationId)
    {
        try
        {
            var subjectEntity = _reservationDAO.GetById(reservationId);

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

    public async Task<IBusinessResult> GetByIdAsync(Guid reservationId)
    {
        try
        {
            var subjectEntity = await _reservationDAO.GetByIdAsync(reservationId);

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
            var subjectEntities = _reservationDAO.GetAll();

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
            var subjectEntities = await _reservationDAO.GetAllAsync();

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
            Expression<Func<Reservation, bool>> filter = null!,
            Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
            string includeProperties = "")
    {
        try
        {
            var subjects = _reservationDAO.GetWithCondition(filter, orderBy, includeProperties);
            var result = _reservationDAO.SaveChanges() > 0;

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
        Expression<Func<Reservation, bool>> filter = null!,
        Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
        string includeProperties = "")
    {
        try
        {
            var subjects = await _reservationDAO.GetWithConditionAsync(filter, orderBy, includeProperties);
            var result = await _reservationDAO.SaveChangesAsync() > 0;

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

    public IBusinessResult Insert(Reservation reservation)
    {
        try
        {
            _reservationDAO.Create(reservation);
            var result = _reservationDAO.SaveChanges() > 0;
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

    public async Task<IBusinessResult> InsertAsync(Reservation reservation)
    {
        try
        {
            await _reservationDAO.CreateAsync(reservation);
            var result = await _reservationDAO.SaveChangesAsync() > 0;
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

    public IBusinessResult Update(Reservation reservation)
    {
        try
        {
            _reservationDAO.Update(reservation);
            var result = _reservationDAO.SaveChanges() > 0;

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

    public async Task<IBusinessResult> UpdateAsync(Reservation reservation)
    {
        try
        {
            await _reservationDAO.UpdateAsync(reservation);
            var result = await _reservationDAO.SaveChangesAsync() > 0;

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
            var subjectEntity = _reservationDAO.GetById(id);

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
}
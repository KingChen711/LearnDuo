using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.DAO;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business;

public class ReservationBusiness : IReservationBusiness
{
    // private readonly ReservationDAO _reservationDAO;
    private readonly UnitOfWork _unitOfWork;

    public ReservationBusiness(NET1704_221_5_TutorDemandContext context)
    {
        // _reservationDAO = new ReservationDAO();
        _unitOfWork ??= new UnitOfWork(context);
    }

    public ReservationBusiness() => _unitOfWork = new UnitOfWork();

    #region Business logic using DAO

    // public IBusinessResult Delete(Guid reservationId)
    // {
    //     try
    //     {
    //         var reservationEntity = _reservationDAO.GetById(reservationId);
    //         if (reservationEntity is null)
    //         {
    //             return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
    //         }
    //         else
    //         {
    //             _reservationDAO.Remove(reservationEntity);
    //             _reservationDAO.SaveChanges();
    //
    //             return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public IBusinessResult Delete(int id)
    // {
    //     try
    //     {
    //         var reservationEntity = _reservationDAO.GetById(id);
    //         if (reservationEntity is null)
    //         {
    //             return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
    //         }
    //         else
    //         {
    //             _reservationDAO.Remove(reservationEntity);
    //             _reservationDAO.SaveChanges();
    //
    //             return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public async Task<IBusinessResult> DeleteAsync(Guid reservationId)
    // {
    //     var reservationEntity = await _reservationDAO.GetByIdAsync(reservationId);
    //     if (reservationEntity is null)
    //     {
    //         return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
    //     }
    //     else
    //     {
    //         _reservationDAO.Remove(reservationEntity);
    //         await _reservationDAO.SaveChangesAsync();
    //
    //         return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
    //     }
    // }
    //
    // public IBusinessResult GetById(Guid reservationId)
    // {
    //     try
    //     {
    //         var reservationEntity = _reservationDAO.GetById(reservationId);
    //
    //         if (reservationEntity is null)
    //         {
    //             return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservationEntity!);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public async Task<IBusinessResult> GetByIdAsync(Guid reservationId)
    // {
    //     try
    //     {
    //         var reservationEntity = await _reservationDAO.GetByIdAsync(reservationId);
    //
    //         if (reservationEntity is null)
    //         {
    //             return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservationEntity!);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public IBusinessResult GetAll()
    // {
    //     try
    //     {
    //         var reservationEntities = _reservationDAO.GetAll();
    //
    //         if (reservationEntities.Any())
    //         {
    //             return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservationEntities!);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public async Task<IBusinessResult> GetAllAsync()
    // {
    //     try
    //     {
    //         var reservationEntities = await _reservationDAO.GetAllAsync();
    //
    //         if (reservationEntities.Any())
    //         {
    //             return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservationEntities!);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    //
    // public IBusinessResult GetWithCondition(
    //         Expression<Func<Reservation, bool>> filter = null!,
    //         Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
    //         string includeProperties = "")
    // {
    //     try
    //     {
    //         var reservations = _reservationDAO.GetWithCondition(filter, orderBy, includeProperties);
    //         var result = _reservationDAO.SaveChanges() > 0;
    //
    //         if (result)
    //         {
    //             return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
    //         }
    //
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public async Task<IBusinessResult> GetWithConditionAysnc(
    //     Expression<Func<Reservation, bool>> filter = null!,
    //     Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
    //     string includeProperties = "")
    // {
    //     try
    //     {
    //         var reservations = await _reservationDAO.GetWithConditionAsync(filter, orderBy, includeProperties);
    //         var result = await _reservationDAO.SaveChangesAsync() > 0;
    //
    //         if (result)
    //         {
    //             return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
    //         }
    //
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public IBusinessResult Insert(Reservation reservation)
    // {
    //     try
    //     {
    //         _reservationDAO.Create(reservation);
    //         var result = _reservationDAO.SaveChanges() > 0;
    //         if (result)
    //         {
    //             return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public async Task<IBusinessResult> InsertAsync(Reservation reservation)
    // {
    //     try
    //     {
    //         await _reservationDAO.CreateAsync(reservation);
    //         var result = await _reservationDAO.SaveChangesAsync() > 0;
    //         if (result)
    //         {
    //             return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public IBusinessResult Update(Reservation reservation)
    // {
    //     try
    //     {
    //         _reservationDAO.Update(reservation);
    //         var result = _reservationDAO.SaveChanges() > 0;
    //
    //         if (result)
    //         {
    //             return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
    //         }
    //
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public async Task<IBusinessResult> UpdateAsync(Reservation reservation)
    // {
    //     try
    //     {
    //         await _reservationDAO.UpdateAsync(reservation);
    //         var result = await _reservationDAO.SaveChangesAsync() > 0;
    //
    //         if (result)
    //         {
    //             return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
    //         }
    //
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }
    //
    // public IBusinessResult GetById(int id)
    // {
    //     try
    //     {
    //         var reservationEntity = _reservationDAO.GetById(id);
    //
    //         if (reservationEntity is not null)
    //         {
    //             return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservationEntity!);
    //         }
    //         else
    //         {
    //             return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
    //     }
    // }

    #endregion

    public IBusinessResult Delete(Guid reservationId)
    {
        try
        {
            var reservationEntity =
                _unitOfWork.ReservationRepository.GetOneWithCondition(r => r.ReservationId.Equals(reservationId));
            if (reservationEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                _unitOfWork.ReservationRepository.PrepareRemove(reservationEntity);
                _unitOfWork.ReservationRepository.Save();

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
            var reservationEntity = _unitOfWork.ReservationRepository.GetById(id);
            if (reservationEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                _unitOfWork.ReservationRepository.PrepareRemove(reservationEntity);
                _unitOfWork.ReservationRepository.Save();

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
        var reservationEntity =
            _unitOfWork.ReservationRepository.GetOneWithCondition(r => r.ReservationId.Equals(reservationId));
        if (reservationEntity is null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        }
        else
        {
            _unitOfWork.ReservationRepository.PrepareRemove(reservationEntity);
            await _unitOfWork.ReservationRepository.SaveAsync();

            return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
        }
    }

    public IBusinessResult GetById(Guid reservationId)
    {
        try
        {
            var reservationEntity = _unitOfWork.ReservationRepository.GetById(reservationId);

            if (reservationEntity is null)
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservationEntity!);
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
            var reservationEntity = await _unitOfWork.ReservationRepository.GetByIdAsync(reservationId);

            if (reservationEntity is null)
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservationEntity!);
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
            var reservationEntities = _unitOfWork.ReservationRepository.GetAll();

            if (reservationEntities.Any())
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservationEntities!);
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
            var reservationEntities = await _unitOfWork.ReservationRepository.GetAllAsync();

            if (reservationEntities.Any())
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservationEntities!);
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


    public async Task<IBusinessResult> FindOneAsync(Expression<Func<Reservation, bool>> condition)
    {
        try
        {
            var tutors = await _unitOfWork.ReservationRepository.GetOneWithConditionAsync(condition);

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

    //public async Task<IBusinessResult> GetWithConditionAysnc(
    //    Expression<Func<reservation, bool>> filter = null!, 
    //    Func<IQueryable<reservation>, IOrderedQueryable<reservation>> orderBy = null!, 
    //    string includeProperties = "")
    //{
    //    try
    //    {
    //        var reservations = await _unitOfWork.ReservationRepository.GetWithConditionAsync(filter, orderBy, includeProperties);
    //        var result = await _unitOfWork.ReservationRepository.SaveAsync() > 0;

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

    public IBusinessResult Create(Reservation reservation)
    {
        try
        {
            _unitOfWork.ReservationRepository.PrepareCreate(reservation);
            var result = _unitOfWork.ReservationRepository.Save() > 0;
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

    public async Task<IBusinessResult> CreateAsync(Reservation reservation)
    {
        try
        {
            var result = await _unitOfWork.ReservationRepository.CreateAsync(reservation) > 0;
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
            _unitOfWork.ReservationRepository.PrepareUpdate(reservation);
            var result = _unitOfWork.ReservationRepository.Save() > 0;

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
            var result = await _unitOfWork.ReservationRepository.UpdateAsync(reservation) > 0;

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
            var reservationEntity = _unitOfWork.ReservationRepository.GetById(id);

            if (reservationEntity is not null)
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservationEntity!);
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
        return _unitOfWork.ReservationRepository.Save();
    }

    public async Task<int> SaveChangeAsync()
    {
        return await _unitOfWork.ReservationRepository.SaveAsync();
    }

    public IBusinessResult GetWithCondition(
        Expression<Func<Reservation, bool>> filter = null!,
        Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
        string includeProperties = "")
    {
        try
        {
            var Reservations = _unitOfWork.ReservationRepository.GetWithCondition(filter, orderBy, includeProperties);

            if (Reservations.Any())
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Reservations);
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
        Expression<Func<Reservation, bool>> filter = null!,
        Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
        string includeProperties = "")
    {
        try
        {
            // Combine the filter with additional includes for TeachingSchedule and Subject
            Expression<Func<Reservation, bool>> combinedFilter = filter;
            string combinedIncludeProperties = includeProperties + ",TeachingSchedule,TeachingSchedule.Subject";

            var reservations = await _unitOfWork.ReservationRepository.GetWithConditionAsync(
                combinedFilter, orderBy, combinedIncludeProperties);

            if (reservations.Any())
            {
                // Successful retrieval
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, reservations);
            }
            else
            {
                // No data found
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }
        catch (Exception ex)
        {
            // Exception occurred
            return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
        }
    }
}
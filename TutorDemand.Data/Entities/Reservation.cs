using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TutorDemand.Data.Enums;

namespace TutorDemand.Data.Entities;

public class Reservation
{
    public int Id { get; set; }

    public Guid ReservationId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid TeachingScheduleId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Giá khóa học phải lớn hơn 0")]
    [DisplayName("Giá khóa học")]
    public int PaidPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    [DisplayName("Trạng thái đặt chỗ")]
    [MaxLength(15)]
    [EnumDataType(typeof(ReservationStatus))]
    public string ReservationStatus { get; set; } = null!;

    [DisplayName("Trạng thái thanh toán")]
    [MaxLength(15)]
    [EnumDataType(typeof(PaymentStatus))]
    public string PaymentStatus { get; set; } = null!;

    [DisplayName("Phương pháp thanh toán")]
    [MaxLength(15)]
    [EnumDataType(typeof(PaymentMethod))]
    public string PaymentMethod { get; set; } = null!;

    //navigator
    public Customer Customer { get; set; } = null!;
    public TeachingSchedule TeachingSchedule { get; set; } = null!;
}
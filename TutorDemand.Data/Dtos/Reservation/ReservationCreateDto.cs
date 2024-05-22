using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TutorDemand.Data.Enums;

namespace TutorDemand.Data.Dtos.Reservation;

public class ReservationCreateDTO
{
    [Required]
    public Guid CustomerId { get; set; }
    [Required]
    public Guid TeachingScheduleId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Giá khóa học phải lớn hơn 0")]
    [DisplayName("Giá khóa học")]
    public int PaidPrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

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
}
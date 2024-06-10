using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TutorDemand.Data.Enums;

namespace TutorDemand.Data.Dtos.Reservation;

public class ReservationUpdateDto
{
   
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
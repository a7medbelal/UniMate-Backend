using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.BookingManagment;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Models.BookingManagement
{
    public class Booking: BaseEntity
    {
        [ForeignKey(nameof(Student))]
        public string? StudentId { get; set; }
        public Student? Student { get; set; }
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public BookingType Type { get; set; }
    }
}

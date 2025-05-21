using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Models.BookingManagement
{
    public class BookBed : Booking
    {
        [ForeignKey(nameof(BookingManagement.Booking))]
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }
        [ForeignKey(nameof(Bed))]
        public int BedId { get; set; }
        public Bed Bed { get; set; }
    }
}

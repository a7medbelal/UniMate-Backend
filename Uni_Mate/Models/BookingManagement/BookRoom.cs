using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Models.BookingManagement
{
    public class BookRoom : Booking
    {
        [ForeignKey(nameof(Booking))]
        public int BookingId { get; set; }
        public Booking booking { get; set; }
        [ForeignKey(nameof(Room))]
        public int? RoomId { get; set; }
        public Room? Room { get; set; }
    }
}

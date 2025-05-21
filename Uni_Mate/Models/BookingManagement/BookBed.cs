using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Models.BookingManagement
{
    public class BookBed:Booking
    {
        [ForeignKey(nameof(Bed))]
        public int? BedId { get; set; }
        public Bed? Bed { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Models.BookingManagement
{
    public class BookApartment : Booking
    {
        [ForeignKey(nameof(Apartment))]
        public int? ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
    }
}

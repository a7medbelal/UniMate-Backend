using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Models.ApartmentManagement
{
    public class FavoriteApartment:BaseEntity
    {
        [ForeignKey(nameof(Apartment))]
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
        [ForeignKey(nameof(Student))]
        public string? StudentId { get; set; }
        public Student? Student { get; set; }
    }
}

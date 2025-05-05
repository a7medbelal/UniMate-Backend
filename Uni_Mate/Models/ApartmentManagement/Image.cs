using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.ApartmentManagement.Enum;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Models.ApartmentManagement
{
    public class Image:BaseEntity
    {
        public string? ImageUrl { get; set; }
        public ImageType? ImageType { get; set; }
        [ForeignKey(nameof(Apartment))]
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
        [ForeignKey(nameof(Room))]
        public int RoomId { get; set; }
        public Room? Room { get; set; }
        [ForeignKey(nameof(Student))]
        public string? StudentId { get; set; }
        public Student? Student { get; set; }
    }
}

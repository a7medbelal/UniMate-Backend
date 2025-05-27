using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Models.Comment_Review
{
    public class Comment : BaseEntity
    {
        public string? Message { get; set; }
        [ForeignKey("Student")]
        public string? StudentId { get; set;}
        public Student? Student { get; set; }
        [ForeignKey("Apartment")]
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
    }
}

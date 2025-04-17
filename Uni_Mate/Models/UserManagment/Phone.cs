using System.ComponentModel.DataAnnotations.Schema;

namespace Uni_Mate.Models.UserManagment
{
    public class Phone : BaseEntity
    {
        public string? PhoneNumber { get; set; }
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}

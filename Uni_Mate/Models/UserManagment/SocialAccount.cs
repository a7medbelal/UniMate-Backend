using System.ComponentModel.DataAnnotations.Schema;

namespace Uni_Mate.Models.UserManagment
{
    public class SocialAccount : BaseEntity
    {
        public string? PlatForm { get; set; }
        public string? Account { get; set; }
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}

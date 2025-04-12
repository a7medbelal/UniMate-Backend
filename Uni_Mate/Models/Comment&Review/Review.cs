using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Models.Comment_Review
{
    public class Review
    {
        public int ReviewId { get; set; }

        public string ReviewerId { get; set; }
        public string? RevieweeUserId { get; set; }
        public int? RevieweeApartmentId { get; set; }

        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public ReviewType ReviewType { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(ReviewerId))]
        public User Reviewer { get; set; }

        [ForeignKey(nameof(RevieweeUserId))]
        public User RevieweeUser { get; set; }
    }
}

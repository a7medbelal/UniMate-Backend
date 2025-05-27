using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.Comment_Review;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Models.UserManagment
{
    public  class User : IdentityUser
    {
      // this for student not the id the id generate randomly  
      public string ?  National_Id { get; set; }

      public string Fname { get; set; }

      public string Lname { get; set; }
        /// <summary>
        /// الصوره الشخصيه لليوزر
        /// </summary>
      public string? Image { get; set; }

      public bool IsActive { get; set; } = false;

      public string? Address { get; set; }
      public string? Governomet { get; set; }

        /// <summary>
        /// this for reset password 
        /// </summary>
        [StringLength(6)]
      public string? ResetPassword { get; set; }
      public DateTime? ResetPasswowrdConfirnation { get; set; }

      public Role role { get; set; }

        //this for review the two relationship with same entity as EF core doesnot understand this so you need make it explicity 
      [InverseProperty(nameof(Review.Reviewer))]
      public ICollection<Review>? ReviewsGiven { get; set; }
     
      [InverseProperty(nameof(Review.RevieweeUser))]
      public ICollection<Review>? ReviewsReceived { get; set; }
      public ICollection<Phone>? Phones { get; set; }
      public ICollection<SocialAccount>? SocialAccounts { get; set; }

    }
}

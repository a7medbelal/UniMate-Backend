using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;
using Uni_Mate.Models.Comment_Review;
using Uni_Mate.Models.GeneralEnum;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Models.UserManagment
{
    public class User : IdentityUser
    {
      public string Fname { get; set; }

      public string Lname { get; set; }
      //public string UserName { get; set; }
        
      public string address { get; set; }

      public string Address { get; set; }
      public string? Image { get; set; }
      public Role role { get; set; }
      public Gender Gender { get; set; }
      public string? Governorate { get; set; }
      // make the user write a short description for himself
      public string? BriefOverView { get; set; }

        //this for review the two relationship with same entity as EF core doesnot understand this so you need make it explicity 
      [InverseProperty(nameof(Review.Reviewer))]
      public ICollection<Review>? ReviewsGiven { get; set; }
     
      [InverseProperty(nameof(Review.RevieweeUser))]
      public ICollection<Review>? ReviewsReceived { get; set; }
      public ICollection<Phone>? Phones { get; set; }
      public ICollection<SocialAccount>? SocialAccounts { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;
using Uni_Mate.Models.Comment_Review;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Models.UserManagment
{
    public class User : IdentityUser
    {
      public string Fname { get; set; }

      public string Lname { get; set; }

      public string UserName { get; set; }
        
      public string address { get; set; }

      public string? image { get; set; }

      

      public Role role { get; set; }

      //this for review the two relationship with same entity as EF core doesnot understand this so you need make it explicity 
      [InverseProperty(nameof(Review.Reviewer))]
      public ICollection<Review> ReviewsGiven { get; set; }
     
      [InverseProperty(nameof(Review.RevieweeUser))]
      public ICollection<Review> ReviewsReceived { get; set; }
    }
}

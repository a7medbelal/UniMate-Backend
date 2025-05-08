using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Models.UserManagment
{
    public class Student : User
    {
        public int YearOfStudy { get; set; }

        public string? University { get; set; }

        public string? Faculty { get; set; }

        public Gender Gender { get; set; }
        public string? Governorate { get; set; }
        // make the user write a short description for himself
        public string? BriefOverView { get; set; }
        // Add any additional properties or methods specific to students here
<<<<<<< HEAD

        public string? FPersonalCardImage { get; set; }
        public string? BPersonalCardImage { get; set; }
=======
         
>>>>>>> b79603c30d7b4ff3aef81344b53e3a27bb14e35b
    }
   
    
}

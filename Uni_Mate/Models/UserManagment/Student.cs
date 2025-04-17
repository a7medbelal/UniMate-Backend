namespace Uni_Mate.Models.UserManagment
{
    public class Student : User
    {
        public int National_Id{ get; set; }
        public int YearOfStudy { get; set; }

        public string? University { get; set; }

        public string? Faculty { get; set; }
        // Add any additional properties or methods specific to students here
    }
   
    
}

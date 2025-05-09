using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Models.UserManagment
{
    public class Student : User
    {
        public int YearOfStudy { get; set; }

        public string? University { get; set; }

        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public string? AcademicYear { get; set; }
        public Gender Gender { get; set; }
        public string? Governorate { get; set; }
        // make the user write a short description for himself
        public string? BriefOverView { get; set; }
        // Add any additional properties or methods specific to students here

        /// <summary>
        /// صورة الكرنية
        /// </summary>
        public string? KarnihImage { get; set; }
        /// <summary>
        /// صورة البطاقة الشخصيةالاماميه
        /// </summary>
        public string? FrontPersonalImage { get; set; }
        /// <summary>
        /// صورة البطاقة الشخصية الخلفيه
        /// </summary>
        public string? BackPersonalImage { get; set; }
    }
   
    
}

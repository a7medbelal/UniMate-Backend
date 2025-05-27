using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Models.Comment_Review;
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
        public ICollection<FavoriteApartment>? FavoriteApartments { get; set; }
        public ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    }
   
    
}

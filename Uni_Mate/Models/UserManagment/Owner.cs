using Uni_Mate.Models.ApartmentManagement;
namespace Uni_Mate.Models.UserManagment
{
    public class Owner : User
    {
        public bool? IsApproved { get; set; }
        
        public string? WhatsappNumber { get; set; }

        public string? ApprovedByAdminId { get; set; }

        public DateTime? ApprovedDate { get; set; }

        
        public User? ApprovedByAdmin { get; set; }
        public ICollection<Apartment>? Apartments { get; set; }
    }
}

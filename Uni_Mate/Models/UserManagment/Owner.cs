namespace Uni_Mate.Models.UserManagment
{
    public class Owner : User
    {
        public bool? IsApproved { get; set; }

        public string? ApprovedByAdminId { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public User ApprovedByAdmin { get; set; }
    }
}

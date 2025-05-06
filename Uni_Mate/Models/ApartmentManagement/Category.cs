namespace Uni_Mate.Models.ApartmentManagement
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Facility> Facilities { get; set; } = new List<Facility>();
    }
}

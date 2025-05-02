namespace Uni_Mate.Models.ApartmentManagement
{
    public class Facility :BaseEntity
    {
        public string Name { get; set; }

        public int FacilityCategoryId { get; set; }
        public Category FacilityCategory { get; set; }

        public ICollection<ApartmentFacility> ApartmentFacilities { get; set; } = new List<ApartmentFacility>();
    }
}

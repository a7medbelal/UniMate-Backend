namespace Uni_Mate.Models.ApartmentManagement
{
    public class ApartmentFacility
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public int FacilityId { get; set; }
        public Facility Facility { get; set; }

        public bool IsAvailable { get; set; }
    }
}
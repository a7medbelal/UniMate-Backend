using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyInfoDisplay.Queries
{
    public class UpdateApartmetInfoDisplayDTO
    {
        public int Id { get; set; }
        public int Num { get; set; }
		public decimal Price { get; set; }
        public string Description { get; set; }
        public string DescripeLocation { get; set; }
        public string Gender { get; set; }
        public string DurationType { get; set; }
        public List<ApartmentFacility> ApartmentFacilities { get; set; }
    }
}

using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.SearchForApartment.Queries
{
    public class GetAparmtmentFilterDTO
    {
        public List<string> Images { get; set; } = [];
        public Gender?  Gender { get; set; }
        public Location? Location { get; set; }
        public string? Floor { get; set; }
        public string? OwnerName { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Price { get; set; }
        public int Capecity { get; internal set; }
    }
}

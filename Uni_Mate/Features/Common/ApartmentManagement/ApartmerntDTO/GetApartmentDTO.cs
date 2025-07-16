using System.Security;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.Common.ApartmentManagement.ApartmerntDTO;

public class GetApartmentDTO
{
    public int Id { get; set; }
    public List<string> Images { get; set; } = [];
    public Location Location { get; set; }
    public string DetailedAddress { get; set; }
    public string Gender { get; set; }
    public List<string> Facilities { get; set; } = [];
    public string Floor { get; set; }
    public string OwnerName { get; set; }
    public int NumberOfRooms { get; set; }
    public decimal? Price { get; set; }
    public bool? Favourite { get; set; }
}

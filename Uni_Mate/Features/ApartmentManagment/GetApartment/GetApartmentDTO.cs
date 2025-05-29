namespace Uni_Mate.Features.ApartmentManagment.GetApartment;

public class GetApartmentDTO
{
    public int Id { get; set; } // Assuming Id is an int, adjust as necessary based on your model
    public List<string> Images { get; set; } = [];
    public string Address { get; set; }
    public string DetailedAddress { get; set; }
    public string Gender { get; set; }
    public List<string> Facilities { get; set; } = [];
    public string Floor { get; set; }
    public string OwnerName { get; set; }
    public int NumberOfRooms { get; set; }
    public decimal Price { get; set; }
    public bool? Favourite { get; set; }
}

namespace Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.ApartmentDTO
{
    public class ApartmentDetailsDTO
    {
        public ApartmentDTO ApartmentDTO { get; set; } = new ApartmentDTO();
        public List<RoomDTO>? Rooms { get; set; }
        public List<ImageApartDTO>? Images { get; set; }
        public Dictionary<string,List<string>>? CategoryWithFacilities { get; set; }
    }
   
    public class RoomDTO
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
    }
    
}

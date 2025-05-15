namespace Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.ApartmentDTO
{
    public class ApartmentDTO
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? DescripeLocation { get; set; }
        public string? Floor { get; set; }
        public int Price { get; set; }
        public string? DurationType { get; set; }
        public bool IsAvailable { get; set; }
        /// <summary>
        /// for boys or girls
        /// </summary>
        public string? kind { get; set; }
        public int RoomCount { get; set; }
        /// <summary>
        // عدد االضيوف
        /// </summary>
        public int BedRoomCount { get; set; }

    }
}

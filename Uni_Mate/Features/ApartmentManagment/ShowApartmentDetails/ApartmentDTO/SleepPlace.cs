namespace Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.ApartmentDTO
{
    /*
     * 1- the details of every room (is bookRoomAvailable , BedRoomAvailable Request
     * 
     */
    public class SleepPlace
    {
        public int RoomId { get; set; }
        public string? ImageRoomUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsFull { get; set; }
        public int NumOfPeople { get; set; }
        public bool RoomRequestAvailable { get; set; }
        public bool BedRequestAvailable { get; set; }

        // show if the user book a room or a bed 
        public List<StudentDTO>? StudentDTOs { get; set; }

    }
}

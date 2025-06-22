namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.AddRoomWithBedsCommands
{
    public class RoomInfoDTO
    {
        public decimal Price { get; set; } // this sum the price of the aparment 
        public int NumberOfBeds { get; set; } // this sum the number of beds in the room
        public int NumberOfRooms { get; set; } // this sum the number of rooms in the apartment 
    }
}

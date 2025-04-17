using System.ComponentModel.DataAnnotations.Schema;

namespace Uni_Mate.Models.ApartmentManagement
{
    public class Bed : BaseEntity
    {
        public double Price { get; set; }
        // to know if there is an empty bed in the room
        public bool IsAvailable { get; set; }
        [ForeignKey(nameof(Room))]
        public int RoomId { get; set; }
        public Room? Room { get; set; }
    }
}

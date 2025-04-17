using System.ComponentModel.DataAnnotations.Schema;

namespace Uni_Mate.Models.ApartmentManagement
{
    public class Room : BaseEntity
    {
        public string? Description { get; set; }
        // remember to update 
        public string? Image { get; set; }
        public bool  IsAvailable { get; set; }
        public int Price { get; set; }

        //Navigational properties
        [ForeignKey(nameof(Apartment))]
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
        public ICollection<Bed>? Beds { get; set; }
    }
}

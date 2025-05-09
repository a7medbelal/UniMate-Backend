using System.ComponentModel.DataAnnotations.Schema;
using Uni_Mate.Models.ApartmentManagement.Enum;

namespace Uni_Mate.Models.ApartmentManagement
{
    public class Image:BaseEntity
    {
        public string? ImageUrl { get; set; }
        public ImageType? ImageType { get; set; }
        [ForeignKey("Apartment")]
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
    }
}

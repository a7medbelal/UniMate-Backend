namespace Uni_Mate.Features.FavoriteManagment.GetFavoriteAparts.Quarry
{
    public class FavoriteApartDTO
    {
        public int ApartmentId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Title { get; set; } 
        public string? Description { get; set; } 
        public decimal Price { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerImage { get; set; }
        public double? Rating { get; set; }
        public bool? IsFavorite { get; set; } = true;
    }
}

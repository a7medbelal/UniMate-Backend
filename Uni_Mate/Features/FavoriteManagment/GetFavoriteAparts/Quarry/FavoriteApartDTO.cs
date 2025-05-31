namespace Uni_Mate.Features.FavoriteManagment.GetFavoriteAparts.Quarry
{
    public class FavoriteApartDTO
    {
        public int ApartmentId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Title { get; set; } 
        public string? Description { get; set; } 
        public string? DescriptionLocation { get; set; }
        public bool? IsFavorite { get; set; } = true;
    }
}

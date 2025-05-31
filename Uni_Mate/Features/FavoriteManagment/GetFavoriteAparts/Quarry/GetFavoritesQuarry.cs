using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.FavoriteManagment.GetFavoriteAparts.Quarry
{
    public record GetFavoritesQuarry() : IRequest<RequestResult<List<FavoriteApartDTO>>>; 

    public class GetFavoritesQuarryHandler : BaseRequestHandler<GetFavoritesQuarry, RequestResult<List<FavoriteApartDTO>>,FavoriteApartment>
    {
        
        public GetFavoritesQuarryHandler(BaseRequestHandlerParameter<FavoriteApartment> parameter):base(parameter)
        {
        }

        public override async Task<RequestResult<List<FavoriteApartDTO>>> Handle(GetFavoritesQuarry request, CancellationToken cancellationToken)
        {
            var userId = _userInfo.ID;
            var favorites =_repository
             .Get(f => f.UserId ==userId)
             .Select(f => new FavoriteApartDTO
             {
                 ApartmentId = f.ApartmentId,
                 ImageUrl = f.Apartment.Images.FirstOrDefault().ImageUrl,
                 Title = $"{f.Apartment.Location} . {f.Apartment.Gender} . {f.Apartment.Floor}",
                 Description = $"{f.Apartment.Rooms.Count} غرف . {f.Apartment.Rooms.SelectMany(r => r.Beds).Count()} ضيف",
                 Price = (decimal)f.Apartment.Price,
                 OwnerName = $"{f.Apartment.Owner.Fname} {f.Apartment.Owner.Lname}",
                 OwnerImage = f.Apartment.Owner.Image,
                 Rating = 5 // لو عندك تقييمات، حط الحسبة هنا
             }).ToList();

            return RequestResult<List<FavoriteApartDTO>>.Success(favorites, "Favorites loaded");
        }
    }
}

using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.GetApartment.Queries;
public record GetApartmentFavoriteQuery(string UserId) : IRequest<RequestResult<HashSet<int>>>;

public class GetApartmentFavoriteQueryHandler : BaseRequestHandler<GetApartmentFavoriteQuery, RequestResult<HashSet<int>>, FavoriteApartment>
{
    public GetApartmentFavoriteQueryHandler(BaseRequestHandlerParameter<FavoriteApartment> parameters) : base(parameters)
    {

    }

    public async override Task<RequestResult<HashSet<int>>> Handle(GetApartmentFavoriteQuery request, CancellationToken cancellationToken)
    {
        HashSet<int> favourites = [];
        var dbFavourites = _repository.Get(i => i.UserId == request.UserId);
        foreach (var fav in dbFavourites)
        {
            favourites.Add(fav.ApartmentId);
        }

        return RequestResult<HashSet<int>>.Success(favourites, "Favourites added successfully"); 
    }
}

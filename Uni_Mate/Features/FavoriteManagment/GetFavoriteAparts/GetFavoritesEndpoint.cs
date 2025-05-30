using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.FavoriteManagment.GetFavoriteAparts.Quarry;

namespace Uni_Mate.Features.FavoriteManagment.GetFavoriteAparts
{
    [Authorize]
    public class GetFavoritesEndpoint : BaseWithoutTRequestEndpoint<List<FavoriteApartDTO>>
    {
        public GetFavoritesEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
        {
        }

        [HttpGet]
        public async Task<EndpointResponse<List<FavoriteApartDTO>>> GetFavorites()
        {
             
            var result = await _mediator.Send(new GetFavoritesQuarry());
            if (result.isSuccess)
            {
                return EndpointResponse<List<FavoriteApartDTO>>.Success(result.data, "Favorites loaded successfully.");
            }
            else
            {
                return EndpointResponse<List<FavoriteApartDTO>>.Failure(result.errorCode, result.message);
            }
        }
    }
}

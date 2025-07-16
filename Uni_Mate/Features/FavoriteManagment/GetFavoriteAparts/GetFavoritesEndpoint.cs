using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.ApartmentManagement.ApartmerntDTO;
using Uni_Mate.Features.FavoriteManagment.GetFavoriteAparts.Quarry;

namespace Uni_Mate.Features.FavoriteManagment.GetFavoriteAparts
{
    [Authorize]
    public class GetFavoritesEndpoint : BaseWithoutTRequestEndpoint<List<GetApartmentDTO>>
    {
        public GetFavoritesEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
        {
        }

        [HttpGet]
        public async Task<EndpointResponse<List<GetApartmentDTO>>> GetFavorites()
        {
             
            var result = await _mediator.Send(new GetFavoritesQuarry());
            if (result.isSuccess)
            {
                return EndpointResponse<List<GetApartmentDTO>>.Success(result.data, "Favorites loaded successfully.");
            }
            else
            {
                return EndpointResponse<List<GetApartmentDTO>>.Failure(result.errorCode, result.message);
            }
        }
    }
}

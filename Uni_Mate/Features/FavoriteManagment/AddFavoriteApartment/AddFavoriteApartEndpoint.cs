using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.FavoriteManagment.AddFavoriteApartment.AddFavoriteApartCommand;

namespace Uni_Mate.Features.FavoriteManagment.AddFavoriteApartment
{
    [Authorize]
    public class AddFavoriteApartEndpoint : BaseEndpoint<AddFavoriteApartVM,bool>
    {
        public AddFavoriteApartEndpoint(BaseEndpointParameters<AddFavoriteApartVM> parameters) : base(parameters) { }

        [HttpPost]
        public async Task<EndpointResponse<bool>> AddFavoriteApartment(AddFavoriteApartVM request)
        {
            var validationResponse = ValidateRequest(request);
            if(validationResponse.isSuccess == false)
            {
                return EndpointResponse<bool>.Failure(validationResponse.errorCode, validationResponse.message);
            }
            var result = await _mediator.Send(new AddFavoriteCommand(request.id));

            if(result.isSuccess)
            {
                return EndpointResponse<bool>.Success(result.data, result.message);
            }
            return EndpointResponse<bool>.Failure(default, result.message);
        }
    }
}

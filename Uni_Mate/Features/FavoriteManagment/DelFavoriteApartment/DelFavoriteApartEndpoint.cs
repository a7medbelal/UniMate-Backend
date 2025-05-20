using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.FavoriteManagment.DelFavoriteApartment.DelFavoriteApartmentCommand;
namespace Uni_Mate.Features.FavoriteManagment.DelFavoriteApartment
{

    [Authorize]
    public class DelFavoriteApartEndpoint : BaseEndpoint<DelFavoriteApartVM, bool>
    {
        public DelFavoriteApartEndpoint(BaseEndpointParameters<DelFavoriteApartVM> parameters) : base(parameters) { }

        [HttpDelete]
        public async Task<EndpointResponse<bool>> DelFavoriteApartment(DelFavoriteApartVM request)
        {
            var validationResponse = ValidateRequest(request);
            if (validationResponse.isSuccess == false)
            {
                return EndpointResponse<bool>.Failure(validationResponse.errorCode, validationResponse.message);
            }
            var result = await _mediator.Send(new DelFavoriteApartCommand(request.id));

            if (result.isSuccess)
            {
                return EndpointResponse<bool>.Success(result.data, result.message);
            }
            return EndpointResponse<bool>.Failure(default, result.message);
        }
    }
}

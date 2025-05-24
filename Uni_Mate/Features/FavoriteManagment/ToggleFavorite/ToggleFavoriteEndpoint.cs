using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.FavoriteManagment.ToggleFavorite.Command;

namespace Uni_Mate.Features.FavoriteManagment.ToggleFavorite
{
    [Authorize]
    public class ToggleFavoriteEndpoint : BaseEndpoint<ToggleFavoriteVM,bool>
    {
        public ToggleFavoriteEndpoint(BaseEndpointParameters<ToggleFavoriteVM> parameters) : base(parameters)
        {
        }
        [HttpPost]
        
        public async Task<EndpointResponse<bool>> ToggleFavorite([FromBody] ToggleFavoriteVM request)
        {
            // check on the validator 
            var valid = ValidateRequest(request);
            if (valid.isSuccess == false)
            {
                return EndpointResponse<bool>.Failure(valid.errorCode, valid.message);
            }

            var result = await _mediator.Send(new ToggleFavoriteCommand(request.id));
            if (result.isSuccess)
            {
                return EndpointResponse<bool>.Success(result.data, result.message);
            }
            return EndpointResponse<bool>.Failure(result.errorCode, result.message);
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.OwnerManager.ToggleActive.Command;

namespace Uni_Mate.Features.OwnerManager.ToggleActive
{
    [Authorize]
    public class ToggleActiveEndpoint : BaseEndpoint<ToggleActiveVM,bool>
    {
        public ToggleActiveEndpoint(BaseEndpointParameters<ToggleActiveVM> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> ToggleActive([FromBody]ToggleActiveVM request)
        {
            var check = ValidateRequest(request);
            if(!check.isSuccess)
            {
                return EndpointResponse<bool>.Failure(check.errorCode, check.message); 
            }

            var result = await _mediator.Send(new ToggleCommand(request.IdUser));
            if (result.isSuccess)
            {
                return EndpointResponse<bool>.Success(true, result.message);
            }

            return EndpointResponse<bool>.Failure(result.errorCode, result.message);
        }
    }
}

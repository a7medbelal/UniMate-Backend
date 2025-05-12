using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.StudentManager.UpdateProfileDisplay.Quarry;

namespace Uni_Mate.Features.StudentManager.UpdateProfileDisplay
{
    [Authorize]
    public class UpdateProfileDisplayEndpoint:BaseWithoutTRequestEndpoint<UpdateProfileDisplayDTO>
    {
        public UpdateProfileDisplayEndpoint(BaseWithoutTRequestEndpointParameters parameters): base(parameters)
        {

        }

        [HttpGet]
        public EndpointResponse<UpdateProfileDisplayDTO> UpdateProfileDisplay()
        {
            var requestResult = _mediator.Send(new UpdateProfileDisplayQuarry()).Result;

            if (!requestResult.isSuccess)
            {
                return EndpointResponse<UpdateProfileDisplayDTO>.Failure(requestResult.errorCode, requestResult.message);
            }
            return EndpointResponse<UpdateProfileDisplayDTO>.Success(requestResult.data, requestResult.message);
        }
    }
}

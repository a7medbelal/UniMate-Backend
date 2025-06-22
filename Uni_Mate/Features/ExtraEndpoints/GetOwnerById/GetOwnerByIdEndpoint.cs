using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ExtraEndpoints.GetOwnerById;
using Uni_Mate.Features.ExtraEndpoints.GetOwnerById.GetOwnerByIdQuarry;

namespace Uni_Mate.Features.ExtraEndpoints.GetOwnerByID
{
    public class GetOwnerByIdEndpoint : BaseEndpoint<GetOwnerByIDVM, GetOwnerByIdDTO>
    {
        public   GetOwnerByIdEndpoint(BaseEndpointParameters<GetOwnerByIDVM> parameters) : base(parameters)
        {
        }

        [HttpGet]

        public async Task<EndpointResponse<GetOwnerByIdDTO>> GetOwner([FromBody] GetOwnerByIDVM request)
        {
            var validationResponse = ValidateRequest(request);
            if (!validationResponse.isSuccess)
            {
                return EndpointResponse<GetOwnerByIdDTO>.Failure(validationResponse.errorCode, validationResponse.message);
            }
            var quarry = new GetOwnerByIdQuarry(request.OwnerId);
            var result = await _mediator.Send(quarry);
            if (!result.isSuccess)
            {
                return EndpointResponse<GetOwnerByIdDTO>.Failure(result.errorCode, result.message);
            }
            return EndpointResponse<GetOwnerByIdDTO>.Success(result.data, "Owner retrieved successfully.");
        }
    }
}

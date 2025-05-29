using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.DeleteFacility.DeletFacilityCommand;

namespace Uni_Mate.Features.ApartmentManagment.DeleteFacility
{
    public class DeleteFacilityEndpoint : BaseEndpoint<DeleteFacilityVM, bool>
    {
        public DeleteFacilityEndpoint(BaseEndpointParameters<DeleteFacilityVM> parameters) : base(parameters)
        {
        }

        [HttpDelete]
        public async Task<EndpointResponse<bool>> DeleteFacility([FromBody] DeleteFacilityVM request)
        {
            var check = ValidateRequest(request);
            if(!check.isSuccess)
            {
                return EndpointResponse<bool>.Failure(check.errorCode, check.message);
            }

            var result = await _mediator.Send(new DeleteFacilityCommand(request.FacilityId));

            if(!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }

            return EndpointResponse<bool>.Success(result.data,result.message);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.StudentManager.ConnectionInfoDisplay.Query;

namespace Uni_Mate.Features.StudentManager.ConnectionInfoDisplay
{
    [Authorize]
    public class ConnectionInfoDisplayEndpoint : BaseWithoutTRequestEndpoint<ConnectionInfoDTO>
    {
        public ConnectionInfoDisplayEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
        {
        }

        [HttpGet]
        public async Task<EndpointResponse<ConnectionInfoDTO>> GetConnectionInfo()
        {
            var result = await _mediator.Send(new ConnectionInfoQuarry());

            if(result.isSuccess)
            {
                return EndpointResponse<ConnectionInfoDTO>.Success(result.data, result.message);
            }
            return EndpointResponse<ConnectionInfoDTO>.Failure(result.errorCode, result.message);
        }
    }
}

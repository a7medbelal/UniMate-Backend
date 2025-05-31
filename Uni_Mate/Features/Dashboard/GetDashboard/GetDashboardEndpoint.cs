using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Dashboard.GetDashboard.Quarry;

namespace Uni_Mate.Features.Dashboard.GetDashboard
{
    [Authorize]
    public class GetDashboardEndpoint : BaseWithoutTRequestEndpoint<GetDashboardDTO>
    {
        public GetDashboardEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
        {
        }
        [HttpGet]
        public async Task<EndpointResponse<GetDashboardDTO>> GetDashboard()
        {
            var result = await _mediator.Send(new GetDashboardQuarry());
            if (result.isSuccess)
                return EndpointResponse<GetDashboardDTO>.Success(result.data, "Dashboard loaded successfully");

            return EndpointResponse<GetDashboardDTO>.Failure(result.errorCode, result.message);
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrelloCopy.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.StudentManager.LoginInfoDisplay.Quarry;
namespace Uni_Mate.Features.StudentManager.LoginInfoDisplay
{
    [Authorize]
    public class LoginInfoDisplayEndpoint : BaseWithoutTRequestEndpoint<LoginInfoDisplayDTO>
    {
        public LoginInfoDisplayEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters) { }


        [HttpGet]
        public async  Task<EndpointResponse<LoginInfoDisplayDTO>> LoginInfoDisplay()
        {
            var result = await _mediator.Send(new LoginInfoDisplayQuarry());
            if (result.isSuccess == true)
            {
                return EndpointResponse<LoginInfoDisplayDTO>.Success(result.data, "Data IS Sent Correctly");
            }
            else
            {
                return EndpointResponse<LoginInfoDisplayDTO>.Failure(result.errorCode, result.message);
            }
        }
    }
}

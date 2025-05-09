using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TrelloCopy.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.StudentManager.UpdateAcademicInfoDisplay.Quarry;

namespace Uni_Mate.Features.StudentManager.UpdateAcademicInfoDisplay
{
    [Authorize]
    public class AcademicInfoDisplayEndpoint : BaseWithoutTRequestEndpoint<AcademicInfoDisplayDTO>
    {

        public AcademicInfoDisplayEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
        {

        }

        [HttpGet]
        public async Task<EndpointResponse<AcademicInfoDisplayDTO>> AcademicInfoDisplay()
        {
            var result = await _mediator.Send(new AcademicInfoDisplayQuarry());

            if (!result.isSuccess)
            {
                return EndpointResponse<AcademicInfoDisplayDTO>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "Student NotFound");

            }
            return EndpointResponse<AcademicInfoDisplayDTO>.Success(result.data, "AcademicInfo  retrieved successfully");
        }
    }
}
            

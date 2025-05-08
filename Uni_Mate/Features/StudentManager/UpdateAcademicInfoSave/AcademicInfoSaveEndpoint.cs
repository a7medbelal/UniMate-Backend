using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.StudentManager.UpdateAcademicInfoSave.Command;

namespace Uni_Mate.Features.StudentManager.UpdateAcademicInfoSave
{
    [Authorize]
    public class AcademicInfoSaveEndpoint : BaseEndpoint<AcademicInfoVM, bool>
    {
        public AcademicInfoSaveEndpoint(BaseEndpointParameters<AcademicInfoVM> parameters) : base(parameters)
        {
        }
        [HttpPost]
        public async Task<EndpointResponse<bool>> UpdateAcademicInfo([FromForm] AcademicInfoVM command)
        {
            var result = await _mediator.Send(new AcademicInfoSaveCommand(
                command.University,
                command.Faculty,
                command.AcademicYear,
                command.Department,
                command.KarnihImage
            ));

            if (!result.isSuccess)
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);

            return EndpointResponse<bool>.Success(result.data, result.message);
        }
    }
}

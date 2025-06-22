using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.StudentManager.UpdateProfileSave.Command;

namespace Uni_Mate.Features.StudentManager.UpdateProfileSave
{
    [Authorize]
    public class UpdateProfileSaveEndpoint : BaseEndpoint<UpdateProfileSaveVM,bool>
    {
        public UpdateProfileSaveEndpoint(BaseEndpointParameters<UpdateProfileSaveVM> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public EndpointResponse<bool> UpdateProfileSave([FromForm] UpdateProfileSaveVM request)
        {
            var result = _validator.Validate(request);
            if(result.IsValid)
            {
                var response = _mediator.Send(new UpdateProfileSaveCommand(request.FirstName, request.LastName, request.Governorate, request.Address, request.BriefOverView ,request.ProfilePic));
                return EndpointResponse<bool>.Success(response.Result.data,response.Result.message);
            }
            else
            {
                var validationError = string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
                return EndpointResponse<bool>.Failure(ErrorCode.InvalidData, validationError);
            }
        }
    }
}

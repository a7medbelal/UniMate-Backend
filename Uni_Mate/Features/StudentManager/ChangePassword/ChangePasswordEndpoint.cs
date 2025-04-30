using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.StudentManager.ChangePassword.Command;
namespace Uni_Mate.Features.StudentManager.ChangePassword
{
    [Authorize]
    public class ChangePasswordEndpoint : BaseEndpoint<ChangePasswordViewModel,bool>
    {
        public ChangePasswordEndpoint(BaseEndpointParameters<ChangePasswordViewModel> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> ChangePassword([FromBody] ChangePasswordViewModel viewmodel)
        {
            var validationResult = ValidateRequest(viewmodel);
            if (!validationResult.isSuccess)
                return validationResult;
            var changePasswordCommand = new ChangePasswordCommand(viewmodel.OldPassword, viewmodel.NewPassword);
            var result = await _mediator.Send(changePasswordCommand);
            if (!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }
            return EndpointResponse<bool>.Success(result.isSuccess, "password changed successfully");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Sig;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Authoraztion.ChangePassword.Command;
using Uni_Mate.Filters;
using Uni_Mate.Models.UserManagment.Enum;
namespace Uni_Mate.Features.Authoraztion.ChangePassword
{
   
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

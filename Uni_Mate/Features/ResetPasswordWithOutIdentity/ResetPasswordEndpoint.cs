using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Authoraztion.ResetPassword.Commands;

namespace Uni_Mate.Features.Authoraztion.ResetPassword
{
    public class ResetPasswordWithOutIdentityEndpoint : BaseEndpoint<ResetPasswordWithOutIdentityViewModel, bool>
    {
        public ResetPasswordWithOutIdentityEndpoint(BaseEndpointParameters<ResetPasswordWithOutIdentityViewModel> parameters):base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> ResetPassword([FromBody] ResetPasswordWithOutIdentityViewModel viewModel)
        {
            var validationResult = ValidateRequest(viewModel);
            if (!validationResult.isSuccess)
                return validationResult;

            var resetPasswordCommand = new ResetPasswordWithOutIdentity(viewModel.Email,viewModel.Token,viewModel.Password,viewModel.ConfirmPassword);
            var result = await _mediator.Send(resetPasswordCommand);
            if (!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }

            return EndpointResponse<bool>.Success(result.isSuccess, result.message);
        }
    }
}

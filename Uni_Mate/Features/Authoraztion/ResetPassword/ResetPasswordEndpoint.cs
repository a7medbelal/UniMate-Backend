using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Authoraztion.ResetPassword.Commands;

namespace Uni_Mate.Features.Authoraztion.ResetPassword
{
    public class ResetPasswordEndpoint : BaseEndpoint<ResetPasswordEndpointViewModel, bool>
    {
        public ResetPasswordEndpoint(BaseEndpointParameters<ResetPasswordEndpointViewModel> parameters):base(parameters)
        {
        }

        [HttpPost]
        public EndpointResponse<bool> ResetPassword([FromBody] ResetPasswordEndpointViewModel viewModel)
        {
            var validationResult = ValidateRequest(viewModel);
            if (!validationResult.isSuccess)
                return validationResult;

            var resetPasswordCommand = new ResetPasswordCommand(viewModel.Token,viewModel.Password,viewModel.ConfirmPassword);
            var result = _mediator.Send(resetPasswordCommand).Result;
            if (!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }

            return EndpointResponse<bool>.Success(result.isSuccess, result.message);
        }
    }
}

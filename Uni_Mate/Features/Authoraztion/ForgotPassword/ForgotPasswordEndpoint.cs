using Azure;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Authoraztion.ForgotPassword.Command;

namespace Uni_Mate.Features.Authoraztion.ForgotPassword
{
    public class ForgotPasswordEndpoint : BaseEndpoint<ForgotPasswordEndpointViewModel,bool>
    {
        public ForgotPasswordEndpoint(BaseEndpointParameters<ForgotPasswordEndpointViewModel>parameters) : base(parameters)
        {
        }

        [HttpPost]
        public EndpointResponse<bool> ForgotPassword([FromBody]ForgotPasswordEndpointViewModel viewModel)
        {
            var validationResult = ValidateRequest(viewModel);

            if(!validationResult.isSuccess)
                return validationResult;
            var forgotPasswordCommand = new ForgotPasswordCommand(viewModel.Email, viewModel.ClientUrl);
            var result = _mediator.Send(forgotPasswordCommand).Result;

            if (!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }
            return EndpointResponse<bool>.Success(result.isSuccess, result.message);

        }
    }
}

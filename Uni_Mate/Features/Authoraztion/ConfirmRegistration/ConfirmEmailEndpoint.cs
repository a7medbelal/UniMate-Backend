using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Features.Authoraztion.LoginUser;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using FluentValidation;
using Uni_Mate.Features.Authoraztion.ConfirmRegistration.Commands;

namespace Uni_Mate.Features.Authoraztion.ConfirmRegistration;

public class ConfirmEmailEndpoint : BaseEndpoint<ConfirmEmailViewModel, bool>
{
    ConfirmEmailEndpoint(BaseEndpointParameters<ConfirmEmailViewModel> parameters) : base(parameters)
    {
    }

    [HttpPost]
    public async Task<EndpointResponse<bool>> ConfirmEmail(ConfirmEmailViewModel viewmodel)
    {
        var validationResult = ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
            return validationResult;
        var confirmationCommand = new ConfirmEmailCommand(viewmodel.Email, viewmodel.Token);
        
        var result = await _mediator.Send(confirmationCommand);
        if (!result.isSuccess)
        {
            return EndpointResponse<bool>.Failure(result.errorCode, result.message);
        }
        return EndpointResponse<bool>.Success(result.isSuccess);
    }
}
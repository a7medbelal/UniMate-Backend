using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Features.Authoraztion.LoginUser;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using FluentValidation;
using Uni_Mate.Features.Authoraztion.ConfirmRegistration.Commands;

namespace Uni_Mate.Features.Authoraztion.ConfirmRegistration;

public class ConfirmEmailEndpoint : BaseEndpoint<ConfirmEmailViewModel, bool>
{
     public ConfirmEmailEndpoint(BaseEndpointParameters<ConfirmEmailViewModel> parameters) : base(parameters)
        {
        }

    [HttpGet]
    public async Task<EndpointResponse<bool>> ConfirmEmail([FromQuery]ConfirmEmailViewModel viewmodel)
    {
        var validationResult = ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
            return validationResult;
        var confirmationCommand = new ConfirmEmailCommand(viewmodel.Email, viewmodel.OTP);
        
        var result = await _mediator.Send(confirmationCommand);
        if (!result.isSuccess)
        {
            return EndpointResponse<bool>.Failure(result.errorCode, result.message);
        }
        return EndpointResponse<bool>.Success(result.isSuccess , "email confrimed sucessfuly you can regiester will ");
    }
}
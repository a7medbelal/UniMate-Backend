using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Authoraztion.LoginUser.Commands;

namespace Uni_Mate.Features.Authoraztion.LoginUser;

public class LogInUserEndpoint : BaseEndpoint<RequestLoginViewModel, LoginResponeViewModel>
{
    public LogInUserEndpoint(BaseEndpointParameters<RequestLoginViewModel> parameters) : base(parameters)
    {
    }

    [HttpPost]
    public async Task<EndpointResponse<LoginResponeViewModel>> LogInUser(RequestLoginViewModel viewmodel)
    {
        var validationResult = ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
            return validationResult;

        var loginCommand = new LoginUserCommand(viewmodel.Email, viewmodel.Password);
        var logInToken = await _mediator.Send(loginCommand);
        if (!logInToken.isSuccess)
            return EndpointResponse<LoginResponeViewModel>.Failure(logInToken.errorCode, logInToken.message);

        var id = logInToken.data.id;
        return EndpointResponse<LoginResponeViewModel>.Success(new LoginResponeViewModel(Token: logInToken.data.Token, Role: logInToken.data.Role, id: id));
    }
}

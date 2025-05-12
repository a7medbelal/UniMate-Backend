    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Uni_Mate.Common.BaseEndpoints;
    using Uni_Mate.Common.BaseEndpoints;
    using Uni_Mate.Common.Views;
    using Uni_Mate.Features.Authoraztion.LogoutUser.Commands;
    using Uni_Mate.Models.UserManagment;
namespace Uni_Mate.Features.Authoraztion.LogoutUser;
    public class LogoutUserEndpoint : BaseWithoutTRequestEndpoint<bool>
    {
        public LogoutUserEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
        {
        }
        [HttpGet]
        public async Task<EndpointResponse<bool>> LogoutUser()
        {
            var result = await _mediator.Send(new LogoutUserCommand());

            if (!result.isSuccess)
                return EndpointResponse<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.UserNotFound, "User not found");

            return EndpointResponse<bool>.Success(true, "Logout successful");
    }
    }
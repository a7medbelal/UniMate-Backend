using MediatR;
using Microsoft.AspNetCore.Identity;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.SendEmailCommand;
using Uni_Mate.Models.UserManagment;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Features.Authoraztion.LoginUser.Commands
{
    public record  LoginUserCommand(string email, string password) : IRequest<RequestResult<TokenDTO>>;

    public class LoginUserCommandHandler : BaseWithoutRepositoryRequestHandler<LoginUserCommand, RequestResult<TokenDTO>>
    {
        public LoginUserCommandHandler(BaseWithotRepositoryRequestHandlerParameters parameters) : base(parameters)
        {
        }

        public async override Task<RequestResult<TokenDTO>>Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.email);

            if (user == null)
                return RequestResult<TokenDTO>.Failure(ErrorCode.UserNotFound, "User not found");

            if (user.EmailConfirmed == false)
                return RequestResult<TokenDTO>.Failure(ErrorCode.EmailNotConfirmed, "Email not confirmed");

            var result = await _userManager.CheckPasswordAsync(user, request.password);

            if (!result)
                return RequestResult<TokenDTO>.Failure(ErrorCode.InvalidPassword, "Invalid password");

            var token = await  _tokenHelper.GenerateToken(user.Id.ToString());

            return RequestResult<TokenDTO>.Success(new TokenDTO(Token : token) );
        }
    }

}

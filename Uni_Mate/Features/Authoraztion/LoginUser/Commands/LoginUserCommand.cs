using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.SendEmailCommand;
using Uni_Mate.Models.UserManagment;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Features.Authoraztion.LoginUser.Commands
{
    public record  LoginUserCommand(string email, string password) : IRequest<RequestResult<TokenDTO>>;

    public class LoginUserCommandHandler : BaseWithoutRepositoryRequestHandler<LoginUserCommand, RequestResult<TokenDTO>, User>
    {
        public LoginUserCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters) : base(parameters)
        {
        }

        public async override Task<RequestResult<TokenDTO>>Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
           // var user = await _userManager.FindByEmailAsync(request.email);
            var UserExist = await _repositoryIdentity.Get(c => c.Email == request.email || c.National_Id == request.email || c.UserName == request.email).Select(c=> new User { 
                
                Email=c.Email,
                Id = c.Id,
                EmailConfirmed = c.EmailConfirmed,
                UserName = c.UserName,
                National_Id = c.National_Id,
                PasswordHash=c.PasswordHash
            }).FirstOrDefaultAsync();

            if (UserExist == null)
                return RequestResult<TokenDTO>.Failure(ErrorCode.UserNotFound, "User not found");

            if (UserExist.EmailConfirmed == false)
                return RequestResult<TokenDTO>.Failure(ErrorCode.EmailNotConfirmed, "Email not confirmed");

            var result = await _userManager.CheckPasswordAsync(UserExist, request.password);

            if (!result)
                return RequestResult<TokenDTO>.Failure(ErrorCode.InvalidPassword, "Invalid password");


            var token = await  _tokenHelper.GenerateToken(UserExist.Id.ToString());

            return RequestResult<TokenDTO>.Success(new TokenDTO(Token : token) );
        }
    }

}

using MediatR;
using TrelloCopy.Common.Views;

namespace Uni_Mate.Features.Authoraztion.RegisterUser.Commands
{
public record RegisterUserCommand(string email, string password, string name, string phoneNo, string country) : IRequest<RequestResult<bool>>;  
}

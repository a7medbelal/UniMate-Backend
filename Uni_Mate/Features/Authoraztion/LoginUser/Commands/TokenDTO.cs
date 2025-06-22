

using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Features.Authoraztion.LoginUser.Commands
{
    public record TokenDTO(string Token ,  Role Role  , string id); 
}

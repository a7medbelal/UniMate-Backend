using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Features.Authoraztion.RoleAcess.Querys
{
    public record HasAccessQuery(Role Role  ,Feature Feature ) :  IRequest<RequestResult<bool>>; 

    public class HasAccessQueryHandler : BaseRequestHandler<HasAccessQuery, RequestResult<bool>,RoleFeature>
    {
        public HasAccessQueryHandler(BaseRequestHandlerParameter<RoleFeature> parameters) : base(parameters)
        {
        }
        public async override Task<RequestResult<bool> >Handle(HasAccessQuery request, CancellationToken cancellationToken)
        {
            var checkTheRole = await _repository.AnyAsync(c => c.role == request.Role && c.feature == request.Feature && !c.Deleted); 
            if (!checkTheRole)
                return RequestResult<bool>.Failure(ErrorCode.Unauthorized, "User not authrized ");

            return RequestResult<bool>.Success(true);
        }
    }


}

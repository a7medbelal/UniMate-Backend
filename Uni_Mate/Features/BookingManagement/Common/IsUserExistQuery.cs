using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Common;

public record IsUserExistQuery(string UserId) : IRequest<RequestResult<bool>>;

public class IsUserExistQueryHandler : BaseWithoutRepositoryRequestHandler<IsUserExistQuery, RequestResult<bool>, User>
{
    public IsUserExistQueryHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters) : base(parameters)
    {
    }

    public override async Task<RequestResult<bool>> Handle(IsUserExistQuery request, CancellationToken cancellationToken)
    {
        var result = await _repositoryIdentity.GetByIDAsync(request.UserId);
        if (result == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "User not found");
        }
        return RequestResult<bool>.Success(true, "User Exists");
    }
}
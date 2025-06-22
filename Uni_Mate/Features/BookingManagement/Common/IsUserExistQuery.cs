using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Common;

public record IsUserExistQuery(string UserId) : IRequest<RequestResult<string>>;

public class IsUserExistQueryHandler : BaseWithoutRepositoryRequestHandler<IsUserExistQuery, RequestResult<string>, Student>
{
    public IsUserExistQueryHandler(BaseWithoutRepositoryRequestHandlerParameters<Student> parameters) : base(parameters)
    {
    }

    public override async Task<RequestResult<string>> Handle(IsUserExistQuery request, CancellationToken cancellationToken)
    {
        var user = await _repositoryIdentity.Get(c=>c.Id == request.UserId).Select(c=>new {c.Email}).FirstOrDefaultAsync();
        if (user is null)
        {
            return RequestResult<string>.Failure(ErrorCode.NotFound, "User not found");
        }

        return RequestResult<string>.Success(user.Email, "User Exists");
    }
}
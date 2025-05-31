using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.OwnerManager.ToggleActive.Command
{
    public record ToggleCommand(string? IdUser) : IRequest<RequestResult<bool>>;

    public class ToggleCommandHandler : BaseWithoutRepositoryRequestHandler<ToggleCommand, RequestResult<bool>,User>
    {
        public ToggleCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters) : base(parameters)
        {
        }
        public override async Task<RequestResult<bool>> Handle(ToggleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.IdUser))
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "The Data Is Not Valid.");
            }

            var user = await _repositoryIdentity.GetByIDAsync(request.IdUser);

            if (user == null)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "The User Not Found.");
            }

            if(user.role == Models.UserManagment.Enum.Role.Admin)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotAvailable, "You Can Not Access An Admin User.");
            }

            user.IsActive = !user.IsActive; 

            await _repositoryIdentity.SaveIncludeAsync(user,nameof(user.IsActive));
            await _repositoryIdentity.SaveChangesAsync();

            return RequestResult<bool>.Success(true,"The Toggle Of The IsActive Status is Changed Succeffully.");
        }
    }
}

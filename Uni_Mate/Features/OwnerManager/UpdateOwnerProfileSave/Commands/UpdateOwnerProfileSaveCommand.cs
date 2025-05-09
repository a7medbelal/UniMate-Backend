using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.OwnerManager.UpdateOwnerProfileSave.Command
{
	public record UpdateOwnerProfileSaveCommand(string? Fname, string? Lname, string? Email, ICollection<string>? Phones, string? BriefOverview) : IRequest<RequestResult<bool>>;

	public class UpdateOwnerProfileSaveCommandHandler : BaseWithoutRepositoryRequestHandler<UpdateOwnerProfileSaveCommand, RequestResult<bool>, Owner>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UpdateOwnerProfileSaveCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<Owner> parameters, IHttpContextAccessor httpContextAccessor) : base(parameters)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public override async Task<RequestResult<bool>> Handle(UpdateOwnerProfileSaveCommand request, CancellationToken cancellationToken)
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return RequestResult<bool>.Failure(ErrorCode.NotFound, "User not found");
			}

			Owner owner = await _repositoryIdentity.GetByIDAsync(userId);
			if (owner == null)
			{
				return RequestResult<bool>.Failure(ErrorCode.NotFound, "Owner not found");
			}

			owner.Fname = request.Fname;
			owner.Lname = request.Lname;
			owner.Email = request.Email;
			owner.PhoneNumber = request.Phones != null ? string.Join(",", request.Phones) : null;
			owner.BriefOverView = request.BriefOverview;

			try
			{
				await _repositoryIdentity.UpdateAsync(owner);
			}
			catch (Exception ex)
			{
				return RequestResult<bool>.Failure(ErrorCode.InternalServerError, $"An error occurred while updating the owner: {ex.Message}");
			}

			return RequestResult<bool>.Success(true, "Owner profile updated successfully");
		}
	}
}
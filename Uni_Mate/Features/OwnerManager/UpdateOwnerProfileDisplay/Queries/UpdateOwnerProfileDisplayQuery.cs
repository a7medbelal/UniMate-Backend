using Mapster;
using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Mapping;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.OwnerManager.GetOwner.Queries;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.OwnerManager.UpdateOwnerProfileDisplay.Queries
{
	public record UpdateOwnerProfileDisplayQuery() : IRequest<RequestResult<GetOwnerDTO>>;

	public class UpdateOwnerProfileDisplayQueryHandler : BaseWithoutRepositoryRequestHandler<UpdateOwnerProfileDisplayQuery, RequestResult<GetOwnerDTO>, Owner>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UpdateOwnerProfileDisplayQueryHandler(BaseWithoutRepositoryRequestHandlerParameters<Owner> parameters, IHttpContextAccessor httpContextAccessor)
			: base(parameters)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public override async Task<RequestResult<GetOwnerDTO>> Handle(UpdateOwnerProfileDisplayQuery request, CancellationToken cancellationToken)
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return RequestResult<GetOwnerDTO>.Failure(ErrorCode.NotFound, "User not found");
			}

			var owner = await _repositoryIdentity.GetByIDAsync(userId);
			if (owner == null)
			{
				return RequestResult<GetOwnerDTO>.Failure(ErrorCode.NotFound, "Owner not found");
			}

			var ownerDto = owner.Adapt<GetOwnerDTO>(MapsterConfig.Configure());
			return RequestResult<GetOwnerDTO>.Success(ownerDto, "Owner profile retrieved successfully");
		}
	}
}
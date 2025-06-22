using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;
using Mapster;
using Uni_Mate.Common.Mapping;

namespace Uni_Mate.Features.OwnerManager.GetOwner.Queries
{
	public record GetOwnerQuery(string OwnerId) : IRequest<RequestResult<GetOwnerDTO>>;

	public class GetOwnerQueryHandler : BaseWithoutRepositoryRequestHandler<GetOwnerQuery, RequestResult<GetOwnerDTO>, Owner>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public GetOwnerQueryHandler(BaseWithoutRepositoryRequestHandlerParameters<Owner> parameters, IHttpContextAccessor httpContextAccessor)
			: base(parameters)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public override async Task<RequestResult<GetOwnerDTO>> Handle(GetOwnerQuery request, CancellationToken cancellationToken)
		{
			Owner owner;
			try
			{
				owner = await _repositoryIdentity.GetByIDAsync(request.OwnerId);
				if (owner == null)
				{
					return RequestResult<GetOwnerDTO>.Failure(ErrorCode.NotFound, "Owner not found");
				}
			}
			catch (Exception ex)
			{
				return RequestResult<GetOwnerDTO>.Failure(ErrorCode.InternalServerError, $"An error occurred while retrieving the owner: {ex.Message}");
			}

			var ownerDto = owner.Adapt<GetOwnerDTO>(MapsterConfig.Configure());
			return RequestResult<GetOwnerDTO>.Success(ownerDto, "Owner retrieved successfully");
		}

	}
}
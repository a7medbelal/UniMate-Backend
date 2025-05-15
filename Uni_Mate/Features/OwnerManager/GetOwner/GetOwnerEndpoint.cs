using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.OwnerManager.GetOwner.Queries;

namespace Uni_Mate.Features.OwnerManager.GetOwner
{
	[Authorize]
	public class GetOwnerEndpoint : BaseWithoutTRequestEndpoint<GetOwnerEndpoint>
	{
		public GetOwnerEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
		{
		}

		[HttpGet]
		public EndpointResponse<GetOwnerDTO> GetOwner()
		{
			var ownerResult = _mediator.Send(new GetOwnerQuery()).Result;
			if (!ownerResult.isSuccess)
			{
				return EndpointResponse<GetOwnerDTO>.Failure(ErrorCode.NotFound, "Owner not found");
			}
			return EndpointResponse<GetOwnerDTO>.Success(ownerResult.data, "Owner retrieved successfully");
		}
	}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.OwnerManager.GetOwner.Queries;

namespace Uni_Mate.Features.OwnerManager.UpdateProfileDisplay
{
	[Authorize]
	public class UpdateOwnerProfileDisplayEndpoint : BaseWithoutTRequestEndpoint<GetOwnerDTO>
	{
		public UpdateOwnerProfileDisplayEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters) 
		{
		}

		[HttpGet]
		public async Task<EndpointResponse<GetOwnerDTO>> UpdateProfileDisplay()
		{
			var result = await _mediator.Send(new GetOwnerQuery());

			if (!result.isSuccess)
			{
				return EndpointResponse<GetOwnerDTO>.Failure(result.errorCode, result.message);
			}

			return EndpointResponse<GetOwnerDTO>.Success(result.data, result.message);
		}
	}
}

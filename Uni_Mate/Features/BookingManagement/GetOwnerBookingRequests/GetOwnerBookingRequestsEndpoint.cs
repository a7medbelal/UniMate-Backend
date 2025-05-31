using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.BookingManagement.GetOwnerBookingRequests.Queries;

namespace Uni_Mate.Features.BookingManagement.GetOwnerBookingRequests
{
	[Authorize]
	public class GetOwnerBookingRequestsEndpoint : BaseWithoutTRequestEndpoint<List<GetOwnerBookingRequestsDTO>>
	{
		public GetOwnerBookingRequestsEndpoint(BaseWithoutTRequestEndpointParameters parameters)
			: base(parameters)
		{
		}

		[HttpGet]
		public async Task<EndpointResponse<List<GetOwnerBookingRequestsDTO>>> GetRequests()
		{
			var result = await _mediator.Send(new GetOwnerBookingRequestsQuery());

			if (!result.isSuccess)
			{
				return EndpointResponse<List<GetOwnerBookingRequestsDTO>>.Failure(result.errorCode, result.message ?? "Failed to retrieve booking requests.");
			}

			return EndpointResponse<List<GetOwnerBookingRequestsDTO>>.Success(result.data, "Requests retrieved successfully");
		}
	}
}
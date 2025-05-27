using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.BookingManagement.GetStudenetBookingRequests.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uni_Mate.Features.BookingManagement.GetStudenetBookingRequests
{
	[Authorize]
	public class GetStudentBookingHistoryEndpoint : BaseWithoutTRequestEndpoint<List<GetStudentBookingHistoryDTO>>
	{
		public GetStudentBookingHistoryEndpoint(BaseWithoutTRequestEndpointParameters parameters)
			: base(parameters)
		{
		}

		[HttpGet]
		public async Task<EndpointResponse<List<GetStudentBookingHistoryDTO>>> GetStudentBookingHistory()
		{
			var result = await _mediator.Send(new GetStudentBookingHistoryQuery());

			if (!result.isSuccess)
			{
				return EndpointResponse<List<GetStudentBookingHistoryDTO>>.Failure(result.errorCode, result.message ?? "Failed to retrieve student booking history.");
			}

			return EndpointResponse<List<GetStudentBookingHistoryDTO>>.Success(result.data, "Student booking history retrieved successfully");
		}
	}
}

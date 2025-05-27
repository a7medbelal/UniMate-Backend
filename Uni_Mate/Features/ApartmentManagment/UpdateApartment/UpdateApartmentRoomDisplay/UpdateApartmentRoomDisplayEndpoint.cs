using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentRoomDisplay.Queries;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentRoomDisplay
{
	[Authorize]
	public class UpdateApartmentRoomDisplayEndpoint : BaseWithoutTRequestEndpoint<UpdateApartmentRoomDisplayDTO>
	{
		public UpdateApartmentRoomDisplayEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
		{
		}

		[HttpGet]
		public async Task<EndpointResponse<UpdateApartmentRoomDisplayDTO>> GetApartmentRoomDisplay(int roomId)
		{
			var result = await _mediator.Send(new UpdateApartmentRoomDisplayQuery(roomId));

			if (!result.isSuccess)
			{
				return EndpointResponse<UpdateApartmentRoomDisplayDTO>.Failure(result.errorCode, result.message ?? "Room not found");
			}

			return EndpointResponse<UpdateApartmentRoomDisplayDTO>.Success(result.data, "Room data retrieved successfully");
		}
	}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.UpdateApartmentRoomSave.Commands;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentRoomSave
{
	[Authorize]
	public class UpdateApartmentExistRoomSaveEndpoint : BaseEndpoint<UpdateApartmentRoomSaveViewModel, bool>
	{
		public UpdateApartmentExistRoomSaveEndpoint(BaseEndpointParameters<UpdateApartmentRoomSaveViewModel> parameters)
			: base(parameters) { }

		[HttpPost]
		public async Task<EndpointResponse<bool>> UpdateApartmentRoomSave([FromForm] UpdateApartmentRoomSaveViewModel request)
		{
			if (!request.RoomId.HasValue)
			{
				// Return an error if RoomId is not provided because update requires RoomId
				return EndpointResponse<bool>.Failure(ErrorCode.InvalidData, "RoomId is required for update.");
			}

			var result = _validator.Validate(request);
			if (!result.IsValid)
			{
				var validationErrors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
				return EndpointResponse<bool>.Failure(ErrorCode.InvalidData, validationErrors);
			}

			var response = await _mediator.Send(new UpdateApartmentExistRoomSaveCommand(
				request.RoomId.Value,
				request.ApartmentId,
				request.Description,
				request.BedCount,
				request.PricePerBed,
				request.HasAC,
				request.RoomPhoto));

			return EndpointResponse<bool>.Success(response.data, response.message);
		}
	}
}
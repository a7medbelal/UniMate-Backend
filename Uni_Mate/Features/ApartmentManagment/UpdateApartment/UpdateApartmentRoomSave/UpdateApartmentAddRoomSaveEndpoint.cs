using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentRoomSave
{
	[Authorize]
	public class UpdateApartmentAddRoomSaveEndpoint : BaseEndpoint<UpdateApartmentRoomSaveViewModel, bool>
	{
		public UpdateApartmentAddRoomSaveEndpoint(BaseEndpointParameters<UpdateApartmentRoomSaveViewModel> parameters)
			: base(parameters) { }

		[HttpPost]
		public async Task<EndpointResponse<bool>> AddApartmentRoomSave([FromForm] UpdateApartmentRoomSaveViewModel request)
		{
			// Validate request data
			var result = _validator.Validate(request);
			if (!result.IsValid)
			{
				var validationErrors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
				return EndpointResponse<bool>.Failure(ErrorCode.InvalidData, validationErrors);
			}

			// Since this is add, RoomId should not be set (or ignored)
			var response = await _mediator.Send(new UpdateApartmentAddRoomSaveCommand(
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

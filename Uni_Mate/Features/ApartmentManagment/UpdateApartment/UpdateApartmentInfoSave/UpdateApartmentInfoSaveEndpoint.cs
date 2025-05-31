using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentInfoSave.Commands;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentInfoSave
{
	[Authorize]
	public class UpdateApartmentInfoSaveEndpoint : BaseEndpoint<UpdateApartmentInfoSaveViewModel, int>
	{
		public UpdateApartmentInfoSaveEndpoint(BaseEndpointParameters<UpdateApartmentInfoSaveViewModel> parameters) : base(parameters)
		{
		}

		[HttpPost]
		public async Task<EndpointResponse<int>> UpdateApartmentInfoSave([FromBody] UpdateApartmentInfoSaveViewModel request)
		{
			var result = _validator.Validate(request);
			if (!result.IsValid)
			{
				var validationErrors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
				return EndpointResponse<int>.Failure(ErrorCode.InvalidData, validationErrors);
			}

			var response = await _mediator.Send(new UpdateApartmentInfoSaveCommand(
				request.ApartmentId,
				request.Price,
				request.Description,
				request.DescripeLocation,
				request.GenderAcceptance,
				request.DurationType));

			return EndpointResponse<int>.Success(response.data, response.message);
		}
	}
}
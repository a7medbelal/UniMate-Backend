using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentInfoDisplay.Queries;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyInfoDisplay.Queries;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyInfoDisplay
{
	[Authorize]
	public class UpdateApartmentInfoDisplayEndpoint : BaseWithoutTRequestEndpoint<UpdateApartmetInfoDisplayDTO>
	{
		public UpdateApartmentInfoDisplayEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
		{
		}

		[HttpGet]
		public async Task<EndpointResponse<UpdateApartmetInfoDisplayDTO>> GetApartmentInfoDisplay(int id)
		{
			var result = await _mediator.Send(new UpdateApartmentInfoDisplayQuery(id));

			if (!result.isSuccess)
			{
				return EndpointResponse<UpdateApartmetInfoDisplayDTO>.Failure(result.errorCode, result.message ?? "Apartment not found");
			}

			return EndpointResponse<UpdateApartmetInfoDisplayDTO>.Success(result.data, "Apartment data retrieved successfully");
		}
	}
}
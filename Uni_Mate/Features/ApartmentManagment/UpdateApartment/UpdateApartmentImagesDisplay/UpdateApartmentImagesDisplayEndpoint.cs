using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentImagesDisplay.Queries;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentImagesDisplay
{
	//[Authorize]
	public class UpdateApartmentImagesDisplayEndpoint : BaseWithoutTRequestEndpoint<GetApartmentImagesDTO>
	{
		public UpdateApartmentImagesDisplayEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
		{
		}

		[HttpGet]
		public async Task<EndpointResponse<GetApartmentImagesDTO>> GetApartmentImages(int id)
		{
			var result = await _mediator.Send(new GetApartmentImagesQuery(id));

			if (!result.isSuccess)
			{
				return EndpointResponse<GetApartmentImagesDTO>.Failure(result.errorCode, result.message ?? "Apartment not found");
			}

			return EndpointResponse<GetApartmentImagesDTO>.Success(result.data, "Apartment images retrieved successfully");
		}
	}
}
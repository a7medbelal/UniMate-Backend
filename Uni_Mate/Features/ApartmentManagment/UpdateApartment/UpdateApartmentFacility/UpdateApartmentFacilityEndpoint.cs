using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentFacility.Commands;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentFacility;
public class UpdateApartmentFacilityEndpoint : BaseEndpoint<List<FacilityApartmentViewModel>, bool>
{
    public UpdateApartmentFacilityEndpoint(BaseEndpointParameters<List<FacilityApartmentViewModel>> parameters) : base(parameters)
    {
    }

    [HttpPost]
    public async Task<EndpointResponse<bool>> UpdateApartmentFacility([FromBody] List<FacilityApartmentViewModel> viewmodel)
    {
        var validationResult = ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
        {
            return validationResult;
        }
        var command = new UpdateApartmentFacilityCommand(viewmodel, 3);
        var result = await _mediator.Send(command);
        if (!result.isSuccess)
        {
            return EndpointResponse<bool>.Failure(result.errorCode, result.message);
        }
        return EndpointResponse<bool>.Success(result.data, "Apartment facilities updated successfully.");
    }
}

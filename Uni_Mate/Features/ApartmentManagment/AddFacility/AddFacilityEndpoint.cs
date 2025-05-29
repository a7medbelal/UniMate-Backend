using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.AddFacility.Command;

namespace Uni_Mate.Features.ApartmentManagment.AddFacility
{
    public class AddFacilityEndpoint:BaseEndpoint<AddFacilityVM, bool>
    {
        public AddFacilityEndpoint(BaseEndpointParameters<AddFacilityVM> parameters) : base(parameters)
        {
        }
        [HttpPost]
        public async Task<EndpointResponse<bool>> AddFacility([FromBody] AddFacilityVM viewModel)
        {
            var validationResult = ValidateRequest(viewModel);
            if (!validationResult.isSuccess)
                return validationResult;
            var command = new AddFacilityCommand(viewModel.Name, viewModel.CategoryId);
            var result = await _mediator.Send(command);
            if (!result.isSuccess)
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            return EndpointResponse<bool>.Success(result.data, "Facility added successfully.");
        }
    }
}

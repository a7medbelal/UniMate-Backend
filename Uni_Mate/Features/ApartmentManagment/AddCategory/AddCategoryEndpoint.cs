using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.AddCategory.Command;

namespace Uni_Mate.Features.ApartmentManagment.AddCategory
{
    [Authorize]
    public class AddCategoryEndpoint :BaseEndpoint<AddCategoryVM, bool>
    {
        public AddCategoryEndpoint(BaseEndpointParameters<AddCategoryVM> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> AddCategory([FromBody] AddCategoryVM viewModel)
        {
            var validationResult = ValidateRequest(viewModel);
            if (!validationResult.isSuccess)
                return validationResult;

            var command = new AddCategoryCommand(viewModel.Name);

            var result = await _mediator.Send(command);
            if (!result.isSuccess)
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            return EndpointResponse<bool>.Success(result.data, "Category added successfully.");
        }
    }
}

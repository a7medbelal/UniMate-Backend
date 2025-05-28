using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.DeleteCategory.Command;

namespace Uni_Mate.Features.ApartmentManagment.DeleteCategory
{
    public class DeleteCategoryEndpoint : BaseEndpoint<DeleteCategoryVM, bool>
    {
        public DeleteCategoryEndpoint(BaseEndpointParameters<DeleteCategoryVM> parameters) : base(parameters)
        {
        }
        [HttpDelete]
        public async Task<EndpointResponse<bool>> DeleteCategory([FromBody] DeleteCategoryVM viewModel)
        {
            var validationResult = ValidateRequest(viewModel);
            if (!validationResult.isSuccess)
                return validationResult;
            var command = new DeleteCategoryCommand(viewModel.Id);
            var result = await _mediator.Send(command);
            if (!result.isSuccess)
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            return EndpointResponse<bool>.Success(result.data, "Category deleted successfully.");
        }
    }
}

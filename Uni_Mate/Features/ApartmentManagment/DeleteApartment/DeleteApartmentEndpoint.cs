using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.DeleteApartment.Commands;

namespace Uni_Mate.Features.ApartmentManagment.DeleteApartment;

public record DeleteApartmnetViewModel(int ApartmentId);
public class DeleteApartmentEndpoint : BaseWithoutTRequestEndpoint<bool>
{
    public DeleteApartmentEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
    {
    }

    [HttpDelete]
    //[ValidateAntiForgeryToken]
    public async Task<EndpointResponse<bool>> DeleteApartment([FromBody] DeleteApartmnetViewModel viewmodel, CancellationToken cancellationToken)
    {
        var deleteApartmentCommand = new DeleteApartmentCommand(viewmodel.ApartmentId);
        var result = await _mediator.Send(deleteApartmentCommand);

        if (!result.isSuccess)
        {
            return EndpointResponse<bool>.Failure(ErrorCode.DeletionFailed, "Failed to delete apartment");
        }
        return EndpointResponse<bool>.Success(true, "Apartment deleted successfully");
    }
}

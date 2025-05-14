using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.UploadPhotoCommand.Commands;

namespace Uni_Mate.Features.Common.UploadPhotoCommand;

public record DeletePhotoViewModel(string ImageUrl);
public class DeletePhotoEndpoint : BaseWithoutTRequestEndpoint<bool>
{
    public DeletePhotoEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
    {
    }
    [HttpDelete]
    public async Task<EndpointResponse<bool>> DeletePhoto([FromBody] DeletePhotoViewModel viewmodel, CancellationToken cancellationToken)
    {
        var deletePhotoCommand = new DeletePhotoCommand(viewmodel.ImageUrl);
        var result = await _mediator.Send(deletePhotoCommand, cancellationToken);
        if (!result.isSuccess)
        {
            return EndpointResponse<bool>.Failure(ErrorCode.NotFound, "Couldn't find image");
        }
        return EndpointResponse<bool>.Success(result.data, "Photo Deleted Successfully");
    }
}

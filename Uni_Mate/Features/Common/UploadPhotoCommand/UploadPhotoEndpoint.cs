using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;

using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Features.Common.UploadPhotoCommand;

public class UploadPhotoEndpoint : BaseWithoutTRequestEndpoint<ImageUploadResult>
{
    public UploadPhotoEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
    {
    }

    [HttpPost]
    public async Task<EndpointResponse<string>> UploadPhoto([FromForm]UploadPhotoViewModel viewmodel, CancellationToken cancellationToken)
    {
        var uploadPhotoCommand = new UploadPhotoCommand(viewmodel.File);
        var result = await _mediator.Send(uploadPhotoCommand);
        if (!result.isSuccess)
        {
            return EndpointResponse<string>.Failure(ErrorCode.InvalidData, "Invalid data");
        }
        return EndpointResponse<string>.Success(result.data, "Photo Uploaded Successfully");
    }
}
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;

using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Features.Common.UploadImageCommand;

public class UploadImageEndpoint : BaseWithoutTRequestEndpoint<ImageUploadResult>
{
    public UploadImageEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
    {
    }

    [HttpPost]
    public async Task<EndpointResponse<string>> UploadImage([FromForm]UploadImageViewModel viewmodel, CancellationToken cancellationToken)
    {
        var uploadImageCommand = new UploadImageCommand(viewmodel.File);
        var result = await _mediator.Send(uploadImageCommand);
        if (!result.isSuccess)
        {
            return EndpointResponse<string>.Failure(ErrorCode.InvalidData, "Invalid data");
        }
        return EndpointResponse<string>.Success(result.data, "Image Uploaded Successfully");
    }
}
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.UploadPhotoCommand.Commands;
namespace Uni_Mate.Features.Common.UploadPhotoCommand;

public record MultipleUploadPhotoViewModel(List<IFormFile> Files);
public class MultiUploadPhotoEndpoint : BaseWithoutTRequestEndpoint<List<string>>
{
    public MultiUploadPhotoEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
    {
    }

    [HttpPost]
    public async Task<EndpointResponse<List<string>>> Handle([FromForm]MultipleUploadPhotoViewModel viewmodel, CancellationToken cancellationToken)
    {
        var multiUploadPhotoCommand = new MultiUploadPhotoCommand(viewmodel.Files);
        var result = await _mediator.Send(multiUploadPhotoCommand, cancellationToken);
        if(!result.isSuccess)
        {
            return EndpointResponse<List<string>>.Failure(ErrorCode.InvalidData, "Failed to upload images.");
        }
        return EndpointResponse<List<string>>.Success(result.data, "Images uploaded successfully");
    }
}

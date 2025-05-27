using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.UploadImageCommand.Commands;
namespace Uni_Mate.Features.Common.UploadImageCommand;

public record MultipleUploadImageViewModel(List<IFormFile> Files);
public class MultiUploadImageEndpoint : BaseWithoutTRequestEndpoint<List<string>>
{
    public MultiUploadImageEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
    {
    }

    [HttpPost]
    public async Task<EndpointResponse<List<string>>> Handle([FromForm]MultipleUploadImageViewModel viewmodel, CancellationToken cancellationToken)
    {
        var multiUploadImageCommand = new MultiUploadImageCommand(viewmodel.Files);
        var result = await _mediator.Send(multiUploadImageCommand, cancellationToken);
        if(!result.isSuccess)
        {
            return EndpointResponse<List<string>>.Failure(ErrorCode.InvalidData, "Failed to upload images.");
        }
        return EndpointResponse<List<string>>.Success(result.data, "Images uploaded successfully");
    }
}

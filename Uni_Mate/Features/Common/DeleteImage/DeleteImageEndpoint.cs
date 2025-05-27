using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.DeleteImage.Commands;
using Uni_Mate.Features.Common.UploadImageCommand.Commands;

namespace Uni_Mate.Features.Common.DeleteImage;

//public record DeleteImageViewModel(string ImageUrl);
//public class DeleteImageEndpoint : BaseWithoutTRequestEndpoint<bool>
//{
//    public DeleteImageEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
//    {
//    }
//    [HttpDelete]
//    public async Task<EndpointResponse<bool>> DeleteImage([FromBody] DeleteImageViewModel viewmodel, CancellationToken cancellationToken)
//    {
//        var deleteImageCommand = new DeleteImageCommand(viewmodel.ImageUrl);
//        var result = await _mediator.Send(deleteImageCommand, cancellationToken);
//        if (!result.isSuccess)
//        {
//            return EndpointResponse<bool>.Failure(ErrorCode.NotFound, "Couldn't find image");
//        }
//        return EndpointResponse<bool>.Success(result.data, "Image Deleted Successfully");
//    }
//}

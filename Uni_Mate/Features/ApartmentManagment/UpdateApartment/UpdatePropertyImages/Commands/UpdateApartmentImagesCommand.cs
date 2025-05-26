using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.ApartmentManagement.UploadApartmentCommand;
using Uni_Mate.Features.Common.DeleteImage.Commands;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyImages.Commands;
public record UpdateApartmentImagesCommand(List<Image> DeleteImages, UploadImagesViewModel UploadedImages) : IRequest<RequestResult<bool>>;

public class UpdateApartmentImagesCommandHandler : BaseRequestHandler<UpdateApartmentImagesCommand, RequestResult<bool>, Image>
{
    public UpdateApartmentImagesCommandHandler(BaseRequestHandlerParameter<Image> parameters) : base(parameters)
    {
    }
    public override async Task<RequestResult<bool>> Handle(UpdateApartmentImagesCommand request, CancellationToken cancellationToken)
    {
        if (request.DeleteImages == null || request.DeleteImages.Count == 0)
        {
            return RequestResult<bool>.Failure(ErrorCode.InvalidData, "No images provided for update");
        }
        foreach (var image in request.DeleteImages)
        {
            var deleteImageCommand = new DeleteImageCommand(image.ImageUrl);
            var result = await _mediator.Send(deleteImageCommand, cancellationToken);
            if (!result.isSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.DeletionFailed, "Failed to delete apartment image");
            }
            await _repository.HardDeleteAsync(image);
        }
        await _repository.SaveChangesAsync();

        return RequestResult<bool>.Success(true, "Images Uploaded successfully");
    }
}

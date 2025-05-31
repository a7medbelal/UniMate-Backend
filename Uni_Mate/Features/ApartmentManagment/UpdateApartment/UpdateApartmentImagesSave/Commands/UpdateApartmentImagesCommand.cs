using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.DeleteApartment.Commands;
using Uni_Mate.Features.Common.ApartmentManagement.UploadApartmentCommand;
using Uni_Mate.Features.Common.DeleteImage.Commands;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyImages.Commands;
public record UpdateApartmentImagesCommand(int ApartmentId, List<string>? DeleteImages, UploadApartmentImagesViewModel? UploadedImages) : IRequest<RequestResult<bool>>;

public class UpdateApartmentImagesCommandHandler : BaseRequestHandler<UpdateApartmentImagesCommand, RequestResult<bool>, Image>
{
    public UpdateApartmentImagesCommandHandler(BaseRequestHandlerParameter<Image> parameters) : base(parameters)
    {
    }
    public override async Task<RequestResult<bool>> Handle(UpdateApartmentImagesCommand request, CancellationToken cancellationToken)
    {
        if (request.UploadedImages != null)
        {
            var uploadImages = new UploadApartmentImagesCommand(request.ApartmentId, request.UploadedImages);
            var uploadImagesResult = await _mediator.Send(uploadImages, cancellationToken);
            
            if(!uploadImagesResult.isSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.UploadFailed, "Failed to upload new apartment images");
            }
        }
        if (request.DeleteImages != null)
        {
            foreach (var image in request.DeleteImages)
            {
                var deleteImageCommand = new DeleteImageCommand(image);
                var result = await _mediator.Send(deleteImageCommand, cancellationToken);
                if (!result.isSuccess)
                {
                    return RequestResult<bool>.Failure(ErrorCode.DeletionFailed, "Failed to delete apartment image");
                }
                await _repository.Get(i => i.ImageUrl == image)
                    .ExecuteDeleteAsync();
            }
        }
        await _repository.SaveChangesAsync();

        return RequestResult<bool>.Success(true, "Images Updated successfully");
    }
}

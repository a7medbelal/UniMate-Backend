using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.DeleteImage.Commands;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.DeleteApartment.Commands;
public record DeleteApartmentImagesCommand(int ApartmentId) : IRequest<RequestResult<List<Image>>>;

public class DeleteApartmentImagesCommandHandler : BaseRequestHandler<DeleteApartmentImagesCommand, RequestResult<List<Image>>, Image>
{
    public DeleteApartmentImagesCommandHandler(BaseRequestHandlerParameter<Image> parameters) : base(parameters)
    {
    }
    public override async Task<RequestResult<List<Image>>> Handle(DeleteApartmentImagesCommand request, CancellationToken cancellationToken)
    {
        List<Image> images = await _repository.GetAll()
            .Where(i => i.ApartmentId == request.ApartmentId)
            .ToListAsync();

        if (images == null)
        {
            return RequestResult<List<Image>>.Failure(ErrorCode.NotFound, "Images not found");
        }
        
        foreach (var image in images)
        {
            var deleteImageCommand = new DeleteImageCommand(image.ImageUrl);
            var result = await _mediator.Send(deleteImageCommand, cancellationToken);
            if (!result.isSuccess)
            {
                return RequestResult<List<Image>>.Failure(ErrorCode.DeletionFailed, "Failed to delete apartment image");
            }
            await _repository.HardDeleteAsync(image);
        }
        var deleteRoomImagesCommand = new DeleteRoomsImagesCommand(request.ApartmentId);
        var roomResult = await _mediator.Send(deleteRoomImagesCommand, cancellationToken);
        if (!roomResult.isSuccess)
        {
            return RequestResult<List<Image>>.Failure(ErrorCode.DeletionFailed, "Failed to delete room images");
        }
        await _repository.SaveChangesAsync();

        return RequestResult<List<Image>>.Success(images, "Images Uploaded successfully");
    }
}

using System.Reflection.Metadata;
using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.DeleteImage.Commands;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.DeleteApartment.Commands;
public record DeleteRoomsImagesCommand(int ApartmentId) : IRequest<RequestResult<bool>>;

public class DeleteRoomsImagesCommandHandler : BaseRequestHandler<DeleteRoomsImagesCommand, RequestResult<bool>, Room>
{
    public DeleteRoomsImagesCommandHandler(BaseRequestHandlerParameter<Room> parameters) : base(parameters)
    {
    }

    public async override Task<RequestResult<bool>> Handle(DeleteRoomsImagesCommand request, CancellationToken cancellationToken)
    {
        var roomsImages = _repository.GetAll()
            .Where(i => i.ApartmentId == request.ApartmentId)
            .Select(i => i.Image)
            .ToList();
        foreach (var roomsImage in roomsImages)
        {
            var deleteImageCommand = new DeleteImageCommand(roomsImage);
            var result = await _mediator.Send(deleteImageCommand, cancellationToken);
            if (!result.isSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.DeletionFailed, "Failed to delete an image");
            }
        }
        return RequestResult<bool>.Success(true, "Images deleted successfully");
    }
}

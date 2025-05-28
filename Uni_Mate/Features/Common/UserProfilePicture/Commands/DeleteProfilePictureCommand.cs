using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.DeleteImage.Commands;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Common.UserProfilePicture.Commands;
public record DeleteProfilePictureCommand() : IRequest<RequestResult<bool>>;

public class DeleteProfilePictureCommandHandler : BaseWithoutRepositoryRequestHandler<DeleteProfilePictureCommand, RequestResult<bool>, User>
{
    public DeleteProfilePictureCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters) : base(parameters)
    {
    }

    public async override Task<RequestResult<bool>> Handle(DeleteProfilePictureCommand request, CancellationToken cancellationToken)
    {
        //var user = await _userManager.FindByIdAsync(_userInfo.ID);
        var user = await _userManager.FindByIdAsync("1");
        string? oldImage = user.Image;

        if (oldImage == null || oldImage == "")
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "Old image not found");
        }
        user.Image = null;

        var deleteImageCommand = new DeleteImageCommand(oldImage);
        var deleteResult = await _mediator.Send(deleteImageCommand, cancellationToken);
        if (!deleteResult.isSuccess)
        {
            return RequestResult<bool>.Failure(ErrorCode.DeleteFailed, "Failed to delete old image");
        }
        return RequestResult<bool>.Success(true, "Message deleted successfully");
    }
}

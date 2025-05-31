using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.DeleteImage.Commands;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Common.UserProfilePicture.Commands;
public record UploadProfilePictureCommand(IFormFile NewImage) : IRequest<RequestResult<bool>>;

public class UploadProfilePictureCommandHandler : BaseWithoutRepositoryRequestHandler<UploadProfilePictureCommand, RequestResult<bool>, User>
{
    public UploadProfilePictureCommandHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters) : base(parameters)
    {
    }

    public override async Task<RequestResult<bool>> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        //var user = await _userManager.FindByIdAsync(_userInfo.ID);
        var user = await _userManager.FindByIdAsync("1");

        if (user == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "User not found");
        }

        var deleteUserImage = new DeleteProfilePictureCommand();
        var deleteResult = await _mediator.Send(deleteUserImage, cancellationToken);

        var uploadImageCommand = new UploadImageCommand.UploadImageCommand(request.NewImage);
        var uploadResult = await _mediator.Send(uploadImageCommand, cancellationToken);
        if (!uploadResult.isSuccess)
        {
            return RequestResult<bool>.Failure(ErrorCode.UploadFailed, "Failed to upload image");
        }
        user.Image = uploadResult.data;
        await _repositoryIdentity.UpdateAsync(user);
        await _repositoryIdentity.SaveChangesAsync();

        return RequestResult<bool>.Success(true, "Profile picture updated successfully");
    }
}

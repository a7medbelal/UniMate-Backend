using CloudinaryDotNet.Actions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.UserProfilePicture.Commands;

namespace Uni_Mate.Features.Common.UserProfilePicture;
//public class UploadProfilePictureEndpoint : BaseEndpoint<UploadProfilePictureViewModel, bool>
//{
//    public UploadProfilePictureEndpoint(BaseEndpointParameters<UploadProfilePictureViewModel> parameters) : base(parameters)
//    {
//    }

//    [HttpPut]
//    public async Task<EndpointResponse<bool>> UploadProfilePicture([FromForm] UploadProfilePictureViewModel viewmodel)
//    {
//        var validationResult = ValidateRequest(viewmodel);

//        var result = await _mediator.Send(new UploadProfilePictureCommand(viewmodel.Image));
//        if (!result.isSuccess)
//            return EndpointResponse<bool>.Failure(ErrorCode.UploadFailed, "Failed to upload image");

//        return EndpointResponse<bool>.Success(result.data, "Profile picture updated successfully");
//    }
//}

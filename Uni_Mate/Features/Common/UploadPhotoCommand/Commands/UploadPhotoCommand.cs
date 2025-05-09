using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.Extensions.Options;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.Common.UploadPhotoCommand;

public record UploadPhotoCommand(IFormFile File) : IRequest<RequestResult<string>>;

public class UploadPhotoCommandHandler : BaseRequestHandler<UploadPhotoCommand, RequestResult<string>, Image>
{
    private readonly Cloudinary _cloudinary;

    public UploadPhotoCommandHandler(BaseRequestHandlerParameter<Image> parameters, IOptions<CloudinarySettings> config) : base(parameters)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }

    // public async Task<ImageUploadResult> UploadPhoto(IFormFile file)
    // {
    //     var uploadResult = new ImageUploadResult();
    //     if (file.Length > 0)
    //     {
    //         using var stream = file.OpenReadStream();
    //         var uploadParams = new ImageUploadParams
    //         {
    //             File = new FileDescription(file.FileName, stream),
    //             Transformation = new Transformation()
    //                 .Height(500).Width(500).Crop("fill").Gravity("face")
    //         };
    //         uploadResult = await _cloudinary.UploadAsync(uploadParams);
    //     }
    //     return uploadResult;
    // }

    // public async Task<DeletionResult> DeletePhoto(string photoId)
    // {
    //     var deletionParams = new DeletionParams(photoId);
    //     var result = await _cloudinary.DestroyAsync(deletionParams);
    //     return result;
    // }
    public override async Task<RequestResult<string>> Handle(UploadPhotoCommand request, CancellationToken cancellationToken)
    {
        if (request.File.Length <= 0)
        {
            return RequestResult<string>.Failure(ErrorCode.NotFound, "Couldn't find image");
        }
        using var stream = request.File.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(request.File.FileName, stream),
            Transformation = new Transformation()
                .Height(500).Width(500).Crop("fill").Gravity("face")
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        string url = uploadResult.SecureUri.ToString();

        if (uploadResult.Error != null)
        {
            return RequestResult<string>.Failure(ErrorCode.NotFound, "Couldn't upload image");
        }
        return RequestResult<string>.Success(url, "Image uploaded successfully");
    }
}
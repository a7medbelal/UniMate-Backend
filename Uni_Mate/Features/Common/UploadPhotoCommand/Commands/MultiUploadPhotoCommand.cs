using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.Extensions.Options;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.Common.UploadPhotoCommand.Commands;
public record MultiUploadPhotoCommand(List<IFormFile> Files) : IRequest<RequestResult<List<string>>>;
public class MultiUploadPhotoCommandHandler : BaseRequestHandler<MultiUploadPhotoCommand, RequestResult<List<string>>, Image>
{
    private readonly Cloudinary _cloudinary;


    public MultiUploadPhotoCommandHandler(BaseRequestHandlerParameter<Image> parameters, IOptions<CloudinarySettings> config) : base(parameters)
    {
        _cloudinary = new Cloudinary(new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret));
    }
    public override async Task<RequestResult<List<string>>> Handle(MultiUploadPhotoCommand request, CancellationToken cancellationToken)
    {
        var result = new List<string>();
        foreach (var file in request.Files)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Transformation = new Transformation()
                    .Height(500).Width(500).Crop("fill").Gravity("face")
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.SecureUrl == null)
            {
                throw new Exception("Upload failed: " + uploadResult.Error?.Message);
            }
            result.Add(uploadResult.SecureUrl.ToString());
        }
        return RequestResult<List<string>>.Success(result);
    }
}

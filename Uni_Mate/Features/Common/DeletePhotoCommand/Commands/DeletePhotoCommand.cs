using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.Extensions.Options;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.Common.DeletePhotoCommand.Commands
{
    public record DeletePhotoCommand(string ImageUrl) : IRequest<RequestResult<bool>>;

    public class DeletePhotoCommandHandler : BaseRequestHandler<DeletePhotoCommand, RequestResult<bool>, Image>
    {
        private readonly Cloudinary _cloudinary;

        public DeletePhotoCommandHandler(BaseRequestHandlerParameter<Image> parameters, IOptions<CloudinarySettings> config) : base(parameters)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }

        private string ExtractPublicIdFromUrl(string url)
        {
            var uri = new Uri(url);
            string[] segments = uri.Segments;
            int uploadIndex = Array.FindIndex(segments, s => s.Trim('/') == "upload");

            if (uploadIndex == -1)
                throw new ArgumentException("URL is not a valid Cloudinary URL");

            List<string> publicIdParts = new List<string>();
            for (int i = uploadIndex + 1; i < segments.Length; i++)
            {
                string segment = segments[i].Trim('/');
                if (segment.StartsWith("v") || segment.Contains("_"))
                    continue;

                publicIdParts.Add(segment);
            }

            string publicId = string.Join("/", publicIdParts);
            publicId = Path.GetFileNameWithoutExtension(publicId);
            return publicId;
        }

        public async override Task<RequestResult<bool>> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            string publicId = ExtractPublicIdFromUrl(request.ImageUrl);

            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            if(result.Error != null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Couldn't find image");
            }
            return RequestResult<bool>.Success(true, "Image deleted successfully");
        }

    }
}

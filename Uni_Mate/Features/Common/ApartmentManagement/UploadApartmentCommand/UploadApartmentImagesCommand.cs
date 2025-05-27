using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Models.ApartmentManagement;
using MediatR;
using Uni_Mate.Features.Common.UploadImageCommand.Commands;
using Uni_Mate.Models.ApartmentManagement.Enum;

namespace Uni_Mate.Features.Common.ApartmentManagement.UploadApartmentCommand;
public record UploadApartmentImagesCommand(int ApartmentId, UploadImagesViewModel Images) : IRequest<RequestResult<List<Image>>>;

public class UploadApartmentImagesCommandHandler : BaseRequestHandler<UploadApartmentImagesCommand, RequestResult<List<Image>>, Image>
{
    public UploadApartmentImagesCommandHandler(BaseRequestHandlerParameter<Image> parameters) : base(parameters)
    {
    }

    public override async Task<RequestResult<List<Image>>> Handle(UploadApartmentImagesCommand request, CancellationToken cancellationToken)
    {
        var result = new List<Image>();
        var imageCategories = new Dictionary<string, List<IFormFile>>
        {
            { "kitchen", request.Images.Kitchen },
            { "bathroom", request.Images.Bathroom },
            { "outside", request.Images.Outside },
            { "living room", request.Images.LivingRoom }
        };

        if (request.Images.Additional != null)
        {
            imageCategories.Add("additional", request.Images.Additional);
        }

        foreach (var category in imageCategories)
        {
            var temp = await _mediator.Send(new MultiUploadImageCommand(category.Value));
            foreach (var obj in temp.data)
            {
                Image image = new Image
                {
                    ImageUrl = obj,
                    ApartmentId = request.ApartmentId,
                    ImageType = category.Key switch
                    {
                        "kitchen" => ImageType.KitchenImage,
                        "bathroom" => ImageType.BathroomImage,
                        "outside" => ImageType.OutsideImage,
                        "living room" => ImageType.LivingRoomImage,
                        "additional" => ImageType.Additional,
                        _ => throw new NotImplementedException(),
                    }
                };
                result.Add(image);
            }
            if (!temp.isSuccess)
            {
                return RequestResult<List<Image>>.Failure(ErrorCode.UploadFailed, $"Failed to upload {category.Key} images");
            }
        }
        await _repository.AddRangeAsync(result);
   

        return RequestResult<List<Image>>.Success(result, "Apartment images sent successfully");
    }
}

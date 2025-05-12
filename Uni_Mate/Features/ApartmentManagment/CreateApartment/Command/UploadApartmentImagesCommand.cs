using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Models.ApartmentManagement;
using MediatR;
using Uni_Mate.Features.Common.UploadPhotoCommand.Commands;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Models.ApartmentManagement.Enum;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartment.Command;
public record UploadApartmentImagesCommand(int ApartmentId, List<IFormFile> Kitchen, List<IFormFile> Bathroom, 
    List<IFormFile> Outside, List<IFormFile> LivingRoom, List<IFormFile>? Additional) : IRequest<RequestResult<List<Image>>>;

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
            { "kitchen", request.Kitchen },
            { "bathroom", request.Bathroom },
            { "outside", request.Outside },
            { "living room", request.LivingRoom }
        };

        if (request.Additional != null)
        {
            imageCategories.Add("additional", request.Additional);
        }

        foreach (var category in imageCategories)
        {
            var temp = await _mediator.Send(new MultiUploadPhotoCommand(category.Value));
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
        await _repository.SaveChangesAsync();

        return RequestResult<List<Image>>.Success(result, "Apartment images sent successfully");
    }
}

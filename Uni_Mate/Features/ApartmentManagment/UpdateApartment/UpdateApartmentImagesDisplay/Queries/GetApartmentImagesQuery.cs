using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.ApartmentManagement.Enum;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentImagesDisplay.Queries;
public record GetApartmentImagesQuery(int ApartmentId) : IRequest<RequestResult<GetApartmentImagesDTO>>;

public class GetApartmentImagesQueryHandler : BaseRequestHandler<GetApartmentImagesQuery, RequestResult<GetApartmentImagesDTO>, Image>
{
    public GetApartmentImagesQueryHandler(BaseRequestHandlerParameter<Image> parameters) : base(parameters)
    {

    }
    public async override Task<RequestResult<GetApartmentImagesDTO>> Handle(GetApartmentImagesQuery request, CancellationToken cancellationToken)
    {
            GetApartmentImagesDTO allImages = new GetApartmentImagesDTO
        {
            Kitchen = [],
            Outside = [],
            Bathroom = [],
            Living = [],
            Additional = []
        };
        var images = await  _repository.Get(i => i.ApartmentId == request.ApartmentId).ToListAsync();
         
        foreach (var image in images)
        {
            if (image != null)
            {

                if (image.ImageType == ImageType.OutsideImage)
                {
                    allImages.Outside.Add(image.ImageUrl);
                }
                else if (image.ImageType == ImageType.LivingRoomImage)
                {
                    allImages.Living.Add(image.ImageUrl);
                }
                else if (image.ImageType == ImageType.BathroomImage)
                {
                    allImages.Bathroom.Add(image.ImageUrl);
                }
                else if (image.ImageType == ImageType.KitchenImage)
                {
                    allImages.Kitchen.Add(image.ImageUrl);
                }
                else
                {
                    allImages.Additional.Add(image.ImageUrl);
                }
            }
        }
        return RequestResult<GetApartmentImagesDTO>.Success(allImages, "here are all images");
    }
}

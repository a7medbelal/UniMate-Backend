using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.CreateApartment.Command;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartment;

public record UploadApartmentImagesViewModel(int ApartmentId, List<IFormFile> Kitchen, List<IFormFile> Bathroom, List<IFormFile> LivingRoom,
    List<IFormFile> Outside, List<IFormFile> Additional);
public class UploadApartmentImagesEndpoint : BaseWithoutTRequestEndpoint<List<Image>>
{
    public UploadApartmentImagesEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
    {
    }
    [HttpPost]
    public async Task<EndpointResponse<List<Image>>> Handle([FromForm] UploadApartmentImagesViewModel viewmodel, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UploadApartmentImagesCommand(
            viewmodel.Kitchen,
            viewmodel.Bathroom,
            viewmodel.Outside,
            viewmodel.LivingRoom,
            viewmodel.Additional
            ));
        if (!result.isSuccess)
        {
            return EndpointResponse<List<Image>>.Failure(ErrorCode.UploadFailed, "Failed to upload apartment images");
        }

        return EndpointResponse<List<Image>>.Success(result.data, "Apartment images uploaded successfully :)");
    }
}

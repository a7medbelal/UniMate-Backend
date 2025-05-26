/**
 * endpoint : /api/apartment/update-images
 * made if you want to test the image update functionality separately
 * uncomment the code below to use it
*/


//using Microsoft.AspNetCore.Mvc;
//using Uni_Mate.Common.BaseEndpoints;
//using Uni_Mate.Common.Views;
//using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyImages.Commands;

//namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyImages;

//public class UpdateApartmentImagesEndpoint : BaseEndpoint<UpdateApartmentImagesViewModel, bool>
//{
//    public UpdateApartmentImagesEndpoint(BaseEndpointParameters<UpdateApartmentImagesViewModel> parameters) : base(parameters)
//    {
//    }
//    [HttpPost]
//    public EndpointResponse<bool> UpdateImages([FromForm] UpdateApartmentImagesViewModel viewmodel, CancellationToken cancellationToken)
//    {
//        var result = _mediator.Send(new UpdateApartmentImagesCommand(, viewmodel.DeletedImages, viewmodel.UploadedImages), cancellationToken).Result;
//        if (!result.isSuccess)
//        {
//            return EndpointResponse<bool>.Failure(result.errorCode, result.message);
//        }
//        return EndpointResponse<bool>.Success(result.data, result.message);
//    }
//}

using Uni_Mate.Features.Common.ApartmentManagement.UploadApartmentCommand;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyImages;
public record UpdateApartmentImagesViewModel(List<Image> DeletedImages, UploadImagesViewModel UploadedImages);
    

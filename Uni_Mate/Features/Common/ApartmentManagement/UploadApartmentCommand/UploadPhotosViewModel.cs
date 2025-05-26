namespace Uni_Mate.Features.Common.ApartmentManagement.UploadApartmentCommand
{
    public record UploadImagesViewModel(List<IFormFile> Kitchen, List<IFormFile> Bathroom,
    List<IFormFile> Outside, List<IFormFile> LivingRoom, List<IFormFile>? Additional);
}

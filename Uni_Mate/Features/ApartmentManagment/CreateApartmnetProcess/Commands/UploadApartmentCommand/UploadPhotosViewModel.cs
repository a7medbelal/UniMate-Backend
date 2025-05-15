namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.UploadApartmentCommand
{
    public record UploadPhotosViewModel(List<IFormFile> Kitchen, List<IFormFile> Bathroom,
    List<IFormFile> Outside, List<IFormFile> LivingRoom, List<IFormFile>? Additional);
}

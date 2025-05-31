namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyImages
{
	public record UploadApartmentImagesViewModel(List<IFormFile>? Kitchen, List<IFormFile>? Bathroom,
List<IFormFile>? Outside, List<IFormFile>? LivingRoom, List<IFormFile>? Additional);
}

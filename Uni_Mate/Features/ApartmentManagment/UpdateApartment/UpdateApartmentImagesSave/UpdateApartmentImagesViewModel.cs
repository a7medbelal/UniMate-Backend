using FluentValidation;
using Uni_Mate.Features.Common.ApartmentManagement.UploadApartmentCommand;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyImages;
public record UpdateApartmentImagesViewModel(int ApartmentId, List<string>? DeletedImages, UploadApartmentImagesViewModel? UploadedImages);
    
public class UpdateApartmentImagesValidator : AbstractValidator<UpdateApartmentImagesViewModel>
{
	public UpdateApartmentImagesValidator()
	{
		RuleFor(x => x.ApartmentId)
			.GreaterThan(0).WithMessage("Apartment ID must be greater than 0");

	}
}
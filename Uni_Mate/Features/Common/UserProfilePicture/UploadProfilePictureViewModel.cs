using FluentValidation;

namespace Uni_Mate.Features.Common.UserProfilePicture;
public record UploadProfilePictureViewModel(IFormFile Image);
public class UploadProfilePictureVieModelValidator : AbstractValidator<UploadProfilePictureViewModel>
{
    public UploadProfilePictureVieModelValidator()
    {
        RuleFor(x => x.Image)
            .Must(file => file.Length <= 5 * 1024 * 1024) // 5 MB limit
            .WithMessage("Image size must not exceed 5 MB.");
    }
}

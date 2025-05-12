using FluentValidation;

namespace Uni_Mate.Features.OwnerManager.UpdateOwnerProfileSave
{
	public record UpdateOwnerProfileSaveViewModel(string? Fname, string? Lname, ICollection<string>? Phones, string? Email, string? BriefOverView);

	public class UpdateOwnerProfileSaveVMValidation : AbstractValidator<UpdateOwnerProfileSaveViewModel>
	{
		public UpdateOwnerProfileSaveVMValidation()
		{
			RuleFor(x => x.Fname)
				.NotEmpty().WithMessage("First name is required.")
				.Length(2, 20).WithMessage("First name must be between 2 and 20 characters.")
				.Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("First name must contain only letters.");

			RuleFor(x => x.Lname)
				.NotEmpty().WithMessage("Last name is required.")
				.Length(2, 20).WithMessage("Last name must be between 2 and 20 characters.")
				.Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("Last name must contain only letters.");

			RuleForEach(x => x.Phones)
				.NotNull().WithMessage("Phone number must not be null.")
				.NotEmpty().WithMessage("Phone number cannot be empty.")
				.Matches(@"^\d{10,15}$").WithMessage("Phone number must be between 10 and 15 digits.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Invalid email format.");

			RuleFor(x => x.BriefOverView)
				.MinimumLength(10).WithMessage("Brief overview must be at least 10 characters.")
				.MaximumLength(200).WithMessage("Brief overview must not exceed 200 characters.");

			RuleFor(x => x.Phones)
				.NotEmpty().WithMessage("At least one phone number is required.");
		}
	}
}
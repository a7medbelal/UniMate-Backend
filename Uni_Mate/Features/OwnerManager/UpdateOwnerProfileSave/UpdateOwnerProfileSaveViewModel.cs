using FluentValidation;

namespace Uni_Mate.Features.OwnerManager.UpdateOwnerProfileSave
{
	public record UpdateOwnerProfileSaveViewModel(string? Fname, string? Lname, string? Adderse,IFormFile image ,  string? BriefOverView , string? government);

	public class UpdateOwnerProfileSaveVMValidation : AbstractValidator<UpdateOwnerProfileSaveViewModel>
	{
		public UpdateOwnerProfileSaveVMValidation()
		{
			RuleFor(x => x.Fname)
			  .Length(2, 20).WithMessage("First name must be between 2 and 20 characters.")
				.Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("First name must contain only letters.");

			RuleFor(x => x.Lname)
				.Length(2, 20).WithMessage("Last name must be between 2 and 20 characters.")
				.Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("Last name must contain only letters.");


			RuleFor(x => x.BriefOverView)
				.MinimumLength(10).WithMessage("Brief overview must be at least 10 characters.")
				.MaximumLength(200).WithMessage("Brief overview must not exceed 200 characters.");
		}
	}
}
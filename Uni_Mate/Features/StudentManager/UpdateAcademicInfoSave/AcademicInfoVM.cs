using FluentValidation;

namespace Uni_Mate.Features.StudentManager.UpdateAcademicInfoSave
{
    public record AcademicInfoVM(
    string? University,
    string? Faculty,
    string? AcademicYear,
    string Department,
    //IFormFile? KarnihImage  
    String? KarnihImage
);

    public class Validator : AbstractValidator<AcademicInfoVM>
    {
        public Validator()
        {
            RuleFor(x => x.University)
                .NotEmpty().WithMessage("University is required.")
                .MaximumLength(100).WithMessage("University name cannot exceed 100 characters.")
				.Matches(@"^[\p{L}\s\u0621-\u064A]+$").WithMessage("‘University name must contain letters and spaces only.");

			RuleFor(x => x.Faculty)
                .NotEmpty().WithMessage("Faculty is required.")
                .MaximumLength(100).WithMessage("Faculty name cannot exceed 100 characters.")
				.Matches(@"^[\p{L}\s\u0621-\u064A]+$").WithMessage("‘Faculty must contain letters and spaces only.");

			RuleFor(x => x.AcademicYear)
                .NotEmpty().WithMessage("Academic Year is required.")
                .Matches(@"^\d{4}/\d{4}$").WithMessage("Academic Year must be in the format YYYY/YYYY.");

            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.")
				.Matches(@"^[\p{L}\s\u0621-\u064A]+$").WithMessage("‘Department must contain letters and spaces only.");
		}
    }
}
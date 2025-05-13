using FluentValidation;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
namespace Uni_Mate.Features.StudentManager.LoginInfoSave
{
    public record LoginInfoSaveVM(string? Email, string? National_Id, IFormFile? FrontImage, IFormFile? BackImage);

    public class Validator : AbstractValidator<LoginInfoSaveVM>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
             .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please provide a valid email address.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Please enter a correctly formatted email address.");
            RuleFor(x => x.National_Id)
             .Cascade(CascadeMode.Stop)
             .NotEmpty().WithMessage("National ID is required")
             .Length(14).WithMessage("National ID must be exactly 14 digits")
             .Matches(@"^\d{14}$").WithMessage("National ID must contain only digits")
             .Custom((nationalId, context) =>
             {
                 var result = BeValidEgyptianId(nationalId);
                 if (!result.isSuccess)
                 {
                     context.AddFailure(result.message); // your custom error
                 }
             });

            RuleFor(x => x.FrontImage)
                .Cascade(CascadeMode.Stop)
               .Must(file=> file==null || BeAValidImage(file)).WithMessage("Front image must be a valid image file.");
            RuleFor(x => x.BackImage)
                .Must(file=> file==null || BeAValidImage(file)).WithMessage("Back image must be a valid image file.");


        }
        private RequestResult<bool> BeValidEgyptianId(string Naltional_ID)
        {
            int century = int.Parse(Naltional_ID[0].ToString());
            int year = int.Parse(Naltional_ID.Substring(1, 2));
            int month = int.Parse(Naltional_ID.Substring(3, 2));
            int day = int.Parse(Naltional_ID.Substring(5, 2));
            int govCode = int.Parse(Naltional_ID.Substring(7, 2));

            // Determine the full year
            int fullYear = century switch
            {
                2 => 1900 + year,
                3 => 2000 + year,
                _ => -1
            };

            if (fullYear == -1) return RequestResult<bool>.Failure(ErrorCode.InvalidNationalId, "Invalid century in National ID");

            // Validate date
            var dob = new DateTime(fullYear, month, day);
            if (dob > DateTime.Now) return RequestResult<bool>.Failure(ErrorCode.InvalidNationalId, "Date of birth cannot be in the future");

            // Validate governorate code
            var validGovCodes = new HashSet<int>
            {
                1, 2, 3, 4, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 21, 22, 23, 24, 25, 26, 27, 28, 29
            };

            if (!validGovCodes.Contains(govCode))
                return RequestResult<bool>.Failure(ErrorCode.InvalidNationalId, "Invalid governorate code in National ID");


            return RequestResult<bool>.Success(true, "Valid National ID");

        }
        private bool BeAValidImage(IFormFile file)
        {
            var validImageTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            if (!validImageTypes.Contains(file.ContentType))
                return false;

            return true;
        }
    }
}

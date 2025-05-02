using FluentValidation;

namespace Uni_Mate.Features.Authoraztion.ChangePassword
{
    public record ChangePasswordViewModel(string OldPassword, string NewPassword,string ConfirmPassword);

    public class ChangePasswordViewModelValidation : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidation()
        {
            RuleFor(x=>x.OldPassword).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Old Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character (!@#$%^&* etc.).");

            RuleFor(x => x.NewPassword).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Old Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character (!@#$%^&* etc.).");


            RuleFor(x => x.ConfirmPassword).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.NewPassword).WithMessage("Confirm Password must match the New Password.");
        }
    }
}

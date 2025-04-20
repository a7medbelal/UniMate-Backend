using Azure;
using FluentValidation;
using Uni_Mate.Common.BaseEndpoints;

namespace Uni_Mate.Features.Authoraztion.ForgotPassword
{
    public record ForgotPasswordEndpointViewModel(string Email, string ClientUrl);

    public class ForgotPasswordEndpointViewModelValidator : AbstractValidator<ForgotPasswordEndpointViewModel>
    {
        public ForgotPasswordEndpointViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is invalid.");

            RuleFor(x => x.ClientUrl).NotEmpty().WithMessage("Url is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("Url is invalid.");
        }
    }
}
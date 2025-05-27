using FluentValidation;
using System.Text.RegularExpressions;

namespace Uni_Mate.Features.StudentManager.ConnectionInfoSave
{
    public record ConnectionInfoVM(
        string? PhoneNum,
        string? AnotherPhoneNum,
        string? WhatAppLink,
        string? FaceBookLink);

    public class ConnectionInfoValidator : AbstractValidator<ConnectionInfoVM>
    {
        public ConnectionInfoValidator()
        {
            // Phone Number Validation
            RuleFor(x => x.PhoneNum)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?[0-9]{8,15}$").WithMessage("Invalid phone number format")
                .When(x => !string.IsNullOrEmpty(x.PhoneNum));

            // Another Phone Number Validation (optional)
            RuleFor(x => x.AnotherPhoneNum)
                .Matches(@"^\+?[0-9]{8,15}$").WithMessage("Invalid secondary phone number format")
                .When(x => !string.IsNullOrEmpty(x.AnotherPhoneNum));

            //// WhatsApp Link Validation
            //RuleFor(x => x.WhatAppLink)
            //    .Must(BeValidWhatsAppLink).WithMessage("Invalid WhatsApp link format")
            //    .When(x => !string.IsNullOrEmpty(x.WhatAppLink));

            //// Facebook Link Validation
            //RuleFor(x => x.FaceBookLink)
            //    .Must(BeValidFacebookLink).WithMessage("Invalid Facebook link format")
            //    .When(x => !string.IsNullOrEmpty(x.FaceBookLink));

            // At least one contact method required
            RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.PhoneNum) ||
                          !string.IsNullOrEmpty(x.WhatAppLink) ||
                          !string.IsNullOrEmpty(x.FaceBookLink))
                .WithMessage("At least one contact method (phone, WhatsApp, or Facebook) is required");
        }

        //private bool BeValidWhatsAppLink(string? link)
        //{
        //    if (string.IsNullOrEmpty(link)) return true;

        //    return Regex.IsMatch(link,
        //        @"^(https?:\/\/)?(www\.)?(wa\.me|api\.whatsapp\.com)\/.+",
        //        RegexOptions.IgnoreCase);
        //}

        //private bool BeValidFacebookLink(string? link)
        //{
        //    if (string.IsNullOrEmpty(link)) return true;

        //    return Regex.IsMatch(link,
        //        @"^(https?:\/\/)?(www\.)?(facebook\.com|fb\.com)\/.+",
        //        RegexOptions.IgnoreCase);
        //}
    }
}
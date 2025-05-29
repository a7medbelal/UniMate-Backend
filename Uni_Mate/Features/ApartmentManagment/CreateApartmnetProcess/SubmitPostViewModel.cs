using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.AddRoomWithBedsCommands;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;
using Uni_Mate.Features.Common.ApartmentManagement.UploadApartmentCommand;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess
{
    public record SubmitPostViewModel(int Num,
            Location Location,
            string Description,
            int Capecity,
            string DescribeLocation,
            string Floor,
            Gender GenderAcceptance,
            ApartmentDurationType DurationType,
            List<RoomBedViewModel> Rooms,
            List<FacilityApartmentViewModel> CategoryFacilities,
            UploadImagesViewModel Images
            );

    public class SubmitPostViewModleValidator : AbstractValidator<SubmitPostViewModel>
    {
        public SubmitPostViewModleValidator()
        {
   //         RuleFor(x => x.Num)
   //          .GreaterThan(0)
   //          .WithMessage("Apartment number must be greater than 0.");

   //         RuleFor(x => x.Location)
   //             .NotEmpty()
   //             .WithMessage("Location is required.")
			//	.Matches(@"^[\p{L}\d\s.,\-_]+$").WithMessage("Location must contain letters, digits, spaces, and allowed punctuation only.");

			//RuleFor(x => x.Description)
   //             .NotEmpty()
   //             .WithMessage("Description is required.")
			//	.Matches(@"^[\p{L}\d\s.,\-_]+$").WithMessage("Description must contain letters, digits, spaces, and allowed punctuation only.");

			//RuleFor(x => x.Floor)
   //             .NotEmpty()
                
   //         RuleFor(x => x.GenderAcceptance)
   //             .IsInEnum()
   //             .WithMessage("Gender acceptance value is invalid.");

   //         RuleFor(x => x.DurationType)
   //             .IsInEnum()
   //             .WithMessage("Duration type is invalid.");

   //         RuleFor(x => x.Rooms)
   //             .NotEmpty()
   //             .WithMessage("At least one room must be added.");

            RuleForEach(x => x.Rooms)
                .SetValidator(new RoomBedViewModeleValidator());

            RuleForEach(x => x.CategoryFacilities)
                .SetValidator(new FacilityApartmentViewModelValidator());
        }
    }

}
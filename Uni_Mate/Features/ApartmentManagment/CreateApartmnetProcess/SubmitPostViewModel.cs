using FluentValidation;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.AddRoomWithBedsCommands;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess
{
    public record SubmitPostViewModel(int Num,
            string Location,
            string Description,
            int Capecity,
            int NumberOfRooms,
            string DescribeLocation,
            string Floor,
            Gender GenderAcceptance,
            ApartmentDurationType DurationType,
            List<RoomBedViewModel> Rooms,
            List<CategoryFacilityViewModel> CategoryFacilities);

    public class SubmitPostViewModleValidator : AbstractValidator<SubmitPostViewModel>
    {
        public SubmitPostViewModleValidator()
        {
            RuleFor(x => x.Num)
             .GreaterThan(0)
             .WithMessage("Apartment number must be greater than 0.");

            RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage("Location is required.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.");

            RuleFor(x => x.Floor)
                .NotEmpty()
                .WithMessage("Floor number is required.");

            RuleFor(x => x.GenderAcceptance)
                .IsInEnum()
                .WithMessage("Gender acceptance value is invalid.");

            RuleFor(x => x.DurationType)
                .IsInEnum()
                .WithMessage("Duration type is invalid.");

            RuleFor(x => x.Rooms)
                .NotEmpty()
                .WithMessage("At least one room must be added.");

            RuleForEach(x => x.Rooms)
                .SetValidator(new RoomBedViewModeleValidator());

            RuleForEach(x => x.CategoryFacilities)
                .SetValidator(new CategoryFacilityViewModelValidator());
        }
    }

}
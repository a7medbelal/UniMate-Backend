﻿using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentRoomSave
{
	public record UpdateApartmentRoomSaveViewModel(
	int? RoomId,  // Because the room might not exist yet, we allow it to be nullable
	int ApartmentId,
	string Description,
	IFormFile? RoomPhoto,
	int BedCount,
	decimal PricePerBed,
	bool HasAC
);


	public class UpdateApartmentRoomSaveViewModelValidator : AbstractValidator<UpdateApartmentRoomSaveViewModel>
	{
		public UpdateApartmentRoomSaveViewModelValidator()
		{
			RuleFor(x => x.RoomId)
				.GreaterThanOrEqualTo(0).WithMessage("Room ID must be zero or greater.");

			RuleFor(x => x.ApartmentId)
				.GreaterThan(0).WithMessage("Apartment ID is required.");

			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Description is required.")
				.MaximumLength(500).WithMessage("Description can't be longer than 500 characters.")
				.Matches(@"^[\p{L}\u0621-\u064A\d .,\-_\\r\\n]+$").WithMessage("Description must contain letters, digits, spaces, and allowed punctuation (. , - _).");

			RuleFor(x => x.BedCount)
				.GreaterThan(0).WithMessage("Bed count must be greater than zero. If you intend to delete the room, please remove it instead of setting the bed count to zero.");

			RuleFor(x => x.PricePerBed)
				.GreaterThan(0).WithMessage("Price per bed must be a positive value.");
		}
	}
}
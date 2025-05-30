using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentInfoSave.Commands
{
	public record UpdateApartmentInfoSaveCommand(

	// Apartment Info to be Edited
	int ApartmentId,
	string Price,
	Location Location,
	string Description,
	string DescripeLocation,
	string Floor,
	Gender GenderAcceptance,
	ApartmentDurationType DurationType
	) : IRequest<RequestResult<int>>;

	public class UpdateApartmentInfoSaveCommandHandler : BaseRequestHandler<UpdateApartmentInfoSaveCommand, RequestResult<int>, Apartment>
	{
		public UpdateApartmentInfoSaveCommandHandler(BaseRequestHandlerParameter<Apartment> parameters) : base(parameters) 
		{
		}

		public override async Task<RequestResult<int>> Handle(UpdateApartmentInfoSaveCommand request, CancellationToken cancellationToken)
		{
			// Check if the user is authorized to update the apartment info
			var ownerID = _userInfo.ID;
			if (string.IsNullOrEmpty(ownerID))
				return RequestResult<int>.Failure(ErrorCode.OwnerNotAuthried, "Owner is not authorized.");

			var apartmentExists = await _repository.Get(a => a.Id == request.ApartmentId && a.OwnerID == ownerID).Select(a => new { a.Id }).FirstOrDefaultAsync();

			if (apartmentExists == null)
				return RequestResult<int>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found or not owned by you");

			// Create a new Apartment object with the updated fields
			var apartmentToUpdate = new Apartment
			{
				Id = request.ApartmentId,
				Location = request.Location,
				Description = request.Description,
				DescripeLocation = request.DescripeLocation,
				Gender = request.GenderAcceptance,
				Floor = request.Floor,
				DurationType = request.DurationType
			};

			// Save changes to the updated fields only
			await _repository.SaveIncludeAsync(apartmentToUpdate,
				nameof(apartmentToUpdate.Location),
				nameof(apartmentToUpdate.Description),
				nameof(apartmentToUpdate.DescripeLocation),
				nameof(apartmentToUpdate.Gender),
				nameof(apartmentToUpdate.Floor),
				nameof(apartmentToUpdate.DurationType)
			);
			await _repository.SaveChangesAsync();
			return RequestResult<int>.Success(request.ApartmentId, "Apartment info updated successfully");
		}

	}
}
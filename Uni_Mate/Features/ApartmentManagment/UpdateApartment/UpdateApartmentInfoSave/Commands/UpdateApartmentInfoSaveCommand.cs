using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentFacility.Commands;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentInfoSave.Commands
{
	public record UpdateApartmentInfoSaveCommand(

	// Apartment Info to be Edited
	int ApartmentId,
	decimal Price,
	string Description,
	string DescripeLocation,
	Gender GenderAcceptance,
	ApartmentDurationType DurationType,
	//int Capacity
	List<FacilityApartmentViewModel> ApartmentFacilities
	) : IRequest<RequestResult<int>>;

	public class UpdateApartmentInfoSaveCommandHandler : BaseRequestHandler<UpdateApartmentInfoSaveCommand, RequestResult<int>, Apartment>
	{
		private IRepository<ApartmentFacility> _apartmentFacilities;
		public UpdateApartmentInfoSaveCommandHandler(BaseRequestHandlerParameter<Apartment> parameters, IRepository<ApartmentFacility> apartmentFacilities) : base(parameters) 
		{
			_apartmentFacilities = apartmentFacilities;
		}

		public override async Task<RequestResult<int>> Handle(UpdateApartmentInfoSaveCommand request, CancellationToken cancellationToken)
		{
			// Check if the user is authorized to update the apartment info
			var ownerID = _userInfo.ID;
			if (string.IsNullOrEmpty(ownerID))
				return RequestResult<int>.Failure(ErrorCode.OwnerNotAuthried, "Owner is not authorized.");

			var apartmentExists = await _repository.Get(a => a.Id == request.ApartmentId).Select(a => new { a.Id }).FirstOrDefaultAsync();

			if (apartmentExists == null)
				return RequestResult<int>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found or not owned by you");

			var apartmentFacilityCommand = new UpdateApartmentFacilityCommand(request.ApartmentFacilities, request.ApartmentId);

			var facilityResult = await _mediator.Send(apartmentFacilityCommand);
			if (!facilityResult.isSuccess)
			{
				return RequestResult<int>.Failure(ErrorCode.NotFound, "apartment facilities not found");
			}

			List<ApartmentFacility> temp = [];
			foreach (var facility in request.ApartmentFacilities)
			{
				temp.Add(new ApartmentFacility
				{
					ApartmentId = request.ApartmentId,
					FacilityId = facility.FacilityId
				});
			}

			// Create a new Apartment object with the updated fields
			var apartmentToUpdate = new Apartment
			{
				Id = request.ApartmentId,
				Description = request.Description,
				DescripeLocation = request.DescripeLocation,
				Gender = request.GenderAcceptance,
				DurationType = request.DurationType,
				ApartmentFacilities = temp
			};

			await _apartmentFacilities.AddRangeAsync(temp);
			// Save changes to the updated fields only
			await _repository.SaveIncludeAsync(apartmentToUpdate,
				nameof(apartmentToUpdate.Description),
				nameof(apartmentToUpdate.DescripeLocation),
				nameof(apartmentToUpdate.Gender),
				nameof(apartmentToUpdate.DurationType)
			);
			await _repository.SaveChangesAsync();
			return RequestResult<int>.Success(request.ApartmentId, "Apartment info updated successfully");
		}

	}
}
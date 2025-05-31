using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentFacility.Commands;
public record UpdateApartmentFacilityCommand(List<FacilityApartmentViewModel> Facilities, int ApartmentID) : IRequest<RequestResult<bool>>;

public class UpdateApartmentFacilityCommandHandler : BaseRequestHandler<UpdateApartmentFacilityCommand, RequestResult<bool>, ApartmentFacility>
{
    public UpdateApartmentFacilityCommandHandler(BaseRequestHandlerParameter<ApartmentFacility> parameters) : base(parameters) { }
    public override async Task<RequestResult<bool>> Handle(UpdateApartmentFacilityCommand request, CancellationToken cancellationToken)
    {
        foreach (var facility in request.Facilities)
        {
            if (facility.IsSelected)
            {
                ApartmentFacility apartmentFacility = new ApartmentFacility
                {
                    ApartmentId = request.ApartmentID,
                    Id = facility.FacilityId
                };
                await _repository.Add(apartmentFacility);
            }
            else
            {
                ApartmentFacility? existingFacility = await _repository.GetByIDAsync(facility.FacilityId);
                await _repository.DeleteAsync(existingFacility);
            }
        }
        return RequestResult<bool>.Success(true, "Facilities updated successfully");
    }
}

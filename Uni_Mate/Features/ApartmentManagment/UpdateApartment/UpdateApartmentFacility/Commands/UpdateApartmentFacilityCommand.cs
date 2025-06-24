using MediatR;
using Microsoft.EntityFrameworkCore;
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
                    Id = request.ApartmentID,
                     FacilityId = facility.FacilityId
                };
                await _repository.Add(apartmentFacility);
            }
            else
            {
                var existingFacility = await _repository.Get(lol => lol.ApartmentId == request.ApartmentID && lol.FacilityId == facility.FacilityId)
                    .ExecuteUpdateAsync(x => x.SetProperty(y => y.Deleted, true));
                //await _repository.DeleteAsync(existingFacility);
            }
        }
        return RequestResult<bool>.Success(true, "Facilities updated successfully");
    }
}

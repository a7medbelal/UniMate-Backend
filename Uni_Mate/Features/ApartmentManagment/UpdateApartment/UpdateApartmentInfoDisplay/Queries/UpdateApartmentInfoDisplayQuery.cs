using MediatR;
using Mapster;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyInfoDisplay.Queries;
using Microsoft.EntityFrameworkCore;


namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentInfoDisplay.Queries
{
	public record UpdateApartmentInfoDisplayQuery(int ApartmetntID) : IRequest<RequestResult<UpdateApartmetInfoDisplayDTO>>;
	public class UpdateApartmentInfoDisplayQueryHandler : BaseRequestHandler<UpdateApartmentInfoDisplayQuery, RequestResult<UpdateApartmetInfoDisplayDTO>, Apartment>
	{
        public UpdateApartmentInfoDisplayQueryHandler(BaseRequestHandlerParameter<Apartment> parameters) : base(parameters)
        {
        }


        public override async Task<RequestResult<UpdateApartmetInfoDisplayDTO>> Handle(UpdateApartmentInfoDisplayQuery request, CancellationToken cancellationToken)
        {
            var userid = _userInfo.ID; 
            var apartment = await _repository.Get(c=> c.Id ==  request.ApartmetntID).Include(c=>c.ApartmentFacilities).FirstOrDefaultAsync();
            if (apartment == null)
            {
                return RequestResult<UpdateApartmetInfoDisplayDTO>.Failure(ErrorCode.NotFound, "Apartment not found");
            }

            if (apartment.OwnerID != _userInfo.ID)
            {
                return RequestResult<UpdateApartmetInfoDisplayDTO>.Failure(ErrorCode.Forbidden, "You are not authorized to access this apartment");
            }

            var dto = new UpdateApartmetInfoDisplayDTO
            {
                Id = apartment.Id,
                Price = apartment.Rooms?.FirstOrDefault()?.Price ?? 0,
                Description = apartment.Description,
                DescripeLocation = apartment.DescripeLocation,
                Gender = apartment.Gender.ToString(),
                DurationType = apartment.DurationType.ToString(),
                ApartmentFacilities = apartment.ApartmentFacilities.Select(f => f.FacilityId).ToList()

            };



            return RequestResult<UpdateApartmetInfoDisplayDTO>.Success(dto);
        }
    }
}
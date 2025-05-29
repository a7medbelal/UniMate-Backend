using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.DeleteFacility.DeletFacilityCommand
{
    public record DeleteFacilityCommand(int id) : IRequest<RequestResult<bool>>;

    public class DeleteFacilityHandler : BaseRequestHandler<DeleteFacilityCommand, RequestResult<bool>, Facility>
    {
        public DeleteFacilityHandler(BaseRequestHandlerParameter<Facility> parameter) : base(parameter) { }

        public override async Task<RequestResult<bool>> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotValide, "The Data Is Not Valid .");
            }

            var facility = await _repository.GetByIDAsync(request.id);
            if (facility == null)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "The Apartment Is Not Found .");
            }

            await _repository.DeleteAsync(facility);
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "The Data Is Deleted Correctly .");
        }
    }
}

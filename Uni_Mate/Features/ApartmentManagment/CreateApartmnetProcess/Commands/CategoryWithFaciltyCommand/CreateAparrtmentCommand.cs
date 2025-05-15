using Mapster;
using MediatR;
using System.Threading;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Mapping;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommands
{


    public record CategoryWithFaciltiesCommand( List<CategoryFacilityViewModel> Categories , int ApartmentID) : IRequest<RequestResult<bool>>;

    public class CategoryWithFaciltyCommandHandler : BaseRequestHandler<CategoryWithFaciltiesCommand, RequestResult<bool>, ApartmentFacility>
    {

        public CategoryWithFaciltyCommandHandler(BaseRequestHandlerParameter<ApartmentFacility> parameters) : base(parameters) { }

        public override async Task<RequestResult<bool>> Handle(CategoryWithFaciltiesCommand request, CancellationToken cancellationToken)
        {
            if (request.ApartmentID < 0)
                return RequestResult<bool>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found");
            // Check if the categories are valid

            var facilites = request.Categories.Adapt<List<ApartmentFacility>>(MapsterConfig.Configure());
            facilites.ForEach(f => f.ApartmentId = request.ApartmentID);

            await _repository.AddRangeAsync(facilites);
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Categories added successfully");
        }
    }


}

using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.AddFacility.Command
{
    public record AddFacilityCommand(string? Name , int CategoryId) : IRequest<RequestResult<bool>>;
   

    public class AddFacilityHandler : BaseRequestHandler<AddFacilityCommand,RequestResult<bool>, Facility>
    {
        private readonly IRepository<Category> _categoryRepo;
        public AddFacilityHandler(BaseRequestHandlerParameter<Facility> paramerters , IRepository<Category> categoryRepo) : base(paramerters)
        {
            _categoryRepo = categoryRepo;
        }

        public override async Task<RequestResult<bool>> Handle(AddFacilityCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(request.Name))
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "Facility name cannot be empty.");
            }
            if (request.CategoryId <= 0)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "Invalid category ID.");
            }

            var category = _categoryRepo.Get(x => x.Id == request.CategoryId);
            if (category == null)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "Category not found.");
            }

            var facility = new Facility
            {
                Name = request.Name,
                FacilityCategoryId = request.CategoryId
            };

            await _repository.AddAsync(facility);
            await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Facility added successfully.");
        }
    }

}

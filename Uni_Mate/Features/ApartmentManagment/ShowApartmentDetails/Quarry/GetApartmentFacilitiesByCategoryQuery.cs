//using MediatR;
//using Uni_Mate.Common.BaseHandlers;
//using Uni_Mate.Common.Views;
//using Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.ApartmentDTO;

//namespace Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.Quarry
//{
//    public record GetApartmentFacilitiesByCategoryQuery(int id) : IRequest<RequestResult<List<CategoryWithFacilitiesDTO>>>;

//    public class GetApartmentFacilitiesByCategoryHandler:BaseWithoutRepositoryRequestHandler<GetApartmentFacilitiesByCategoryQuery,RequestResult<List<CategoryWithFacilitiesDTO>>,Facilites>
//    {
//        public GetApartmentFacilitiesByCategoryHandler(BaseRequestHandlerParameter<Facilites> parameters) : base(parameters)
//        {
//        }
//        public override async Task<RequestResult<List<CategoryWithFacilitiesDTO>>> Handle(GetApartmentFacilitiesByCategoryQuery request, CancellationToken cancellationToken)
//        {
//            if (request.id <= 0)
//            {
//                return RequestResult<List<CategoryWithFacilitiesDTO>>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "Apartment ID Is Invalid");
//            }
//            var apartment = await _repository.GetWithIncludeAsync(request.id, "Facilities");
//            if (apartment == null)
//            {
//                return RequestResult<List<CategoryWithFacilitiesDTO>>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "The Apartment Not Found");
//            }
//            var categories = apartment.Facilities
//                .GroupBy(f => f.CategoryId)
//                .Select(g => new CategoryWithFacilitiesDTO
//                {
//                    CategoryId = g.Key,
//                    CategoryName = g.First().Category.Name,
//                    Facilities = g.Select(f => new FacilityDTO
//                    {
//                        Id = f.Id,
//                        Name = f.Name
//                    }).ToList()
//                }).ToList();
//            return RequestResult<List<CategoryWithFacilitiesDTO>>.Success(categories);
//        }
//    }
//}

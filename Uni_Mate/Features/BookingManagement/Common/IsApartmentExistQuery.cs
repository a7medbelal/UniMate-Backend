using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Features.BookingManagement.Common
{
    public record IsApartmentExistQuery(int ApartmentId) : IRequest<RequestResult<bool>>;

    public class  IsApartmentExistHandler : BaseRequestHandler<IsApartmentExistQuery, RequestResult<bool>,Models.ApartmentManagement.Apartment>
    {
        public IsApartmentExistHandler(BaseRequestHandlerParameter<Models.ApartmentManagement.Apartment> parameters) : base(parameters)
        {
        }

        public override async Task<RequestResult<bool>> Handle(IsApartmentExistQuery request, CancellationToken cancellationToken)
        {
            if(request.ApartmentId <= 0)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotValide, "Invalid Apartment ID");
            }

            var apartment = await _repository.GetByIDAsync(request.ApartmentId);
            if (apartment == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Apartment not found");
            }

            return RequestResult<bool>.Success(true, "Apartment exists and is valid");
        }
    }
}

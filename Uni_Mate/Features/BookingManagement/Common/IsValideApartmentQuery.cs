using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.BookingManagement.Common;

public record IsValideApartmentQuery(int ApartmentId) : IRequest<RequestResult<bool>>;

public class IsValideApartmentQueryHandler : BaseRequestHandler<IsValideApartmentQuery, RequestResult<bool>, Apartment>
{
    public IsValideApartmentQueryHandler(BaseRequestHandlerParameter<Apartment> parameters) : base(parameters)
    {
    }

    public async override Task<RequestResult<bool>> Handle(IsValideApartmentQuery request, CancellationToken cancellationToken)
    {
        var apart = await _repository.GetByIDAsync(request.ApartmentId);

        if (apart == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "Apartment not found");
        }

        if (apart.IsAvailable == false)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotValide, "The Apartment is not valide .");
        }

        return RequestResult<bool>.Success(true, "Apartment exists");
    }
}
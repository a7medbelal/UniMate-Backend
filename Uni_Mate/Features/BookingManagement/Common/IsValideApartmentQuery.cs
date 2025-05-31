using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Features.BookingManagement.Common;

public record IsValideApartmentQuery(int ApartmentId) : IRequest<RequestResult<bool>>;

public class IsValideApartmentQueryHandler : BaseRequestHandler<IsValideApartmentQuery, RequestResult<bool>, Uni_Mate.Models.ApartmentManagement.Apartment>
{
    public IsValideApartmentQueryHandler(BaseRequestHandlerParameter<Models.ApartmentManagement.Apartment> parameters) : base(parameters)
    {
    }

    public async override Task<RequestResult<bool>> Handle(IsValideApartmentQuery request, CancellationToken cancellationToken)
    {
        var apart = await _repository.Get(c=>c.Id == request.ApartmentId).Select(c=>new { c.IsAvailable , c.Id} ).FirstOrDefaultAsync();

        if (apart is null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "Apartment not found");
        }

        if (apart.IsAvailable == false)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotValide, "The Apartment is not Available .");
        }

        return RequestResult<bool>.Success(true, "Apartment exists");
    }
}
using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.BookingManagement.Common;

public record IsBedExistQuery(int BedId) : IRequest<RequestResult<bool>>;

public class IsBedExistQueryHandler : BaseRequestHandler<IsBedExistQuery, RequestResult<bool>, Bed>
{
    public IsBedExistQueryHandler(BaseRequestHandlerParameter<Bed> parameters) : base(parameters)
    {
    }

    public async override Task<RequestResult<bool>> Handle(IsBedExistQuery request, CancellationToken cancellationToken)
    {
        var bed = await _repository.GetByIDAsync(request.BedId);
        if (bed == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "Bed not found");
        }

        if (!bed.IsAvailable)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotValide, "Bed is not valide");
        }
        return RequestResult<bool>.Success(true, "Bed exists");
    }
}

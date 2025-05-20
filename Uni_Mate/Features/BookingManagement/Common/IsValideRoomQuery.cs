using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.Common;

public record IsValideRoomQuery(int RoomId) : IRequest<RequestResult<bool>>;

public class IsValideRoomQueryHandler : BaseRequestHandler<IsValideRoomQuery, RequestResult<bool>, Room>
{
    public IsValideRoomQueryHandler(BaseRequestHandlerParameter<Room> parameters) : base(parameters)
    {
    }

    public async override Task<RequestResult<bool>> Handle(IsValideRoomQuery request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIDAsync(request.RoomId);

        if (room == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "Room not found");
        }
        if (room.IsAvailable == false)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotValide, "Room is not valide");
        }
        return RequestResult<bool>.Success(true, "Room exists");
        throw new NotImplementedException();
    }
}
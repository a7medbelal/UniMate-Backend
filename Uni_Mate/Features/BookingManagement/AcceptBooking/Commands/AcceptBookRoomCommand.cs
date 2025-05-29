using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Models.BookingManagment;

namespace Uni_Mate.Features.BookingManagement.AcceptBooking.Commands;
public record AcceptBookRoomCommand(int RoomId, int BookingId) : IRequest<RequestResult<bool>>;

public class AcceptBookRoomCommandHandler : BaseRequestHandler<AcceptBookRoomCommand, RequestResult<bool>, BookRoom>
{
    private readonly IRepository<Uni_Mate.Models.ApartmentManagement.Room> _roomRepository;
    private readonly IRepository<Apartment> _apartmentRepository;
    public AcceptBookRoomCommandHandler(BaseRequestHandlerParameter<BookRoom> parameter, IRepository<Uni_Mate.Models.ApartmentManagement.Room> repository, IRepository<Apartment> apartmentRepository) : base(parameter)
    {
        _roomRepository = repository;
        _apartmentRepository = apartmentRepository;
    }
    public async override Task<RequestResult<bool>> Handle(AcceptBookRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.Get(i => i.Id == request.RoomId).FirstOrDefaultAsync();
        if (room == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "Room not found");
        }
        if (room.IsAvailable == false)
        {
            return RequestResult<bool>.Failure(ErrorCode.RoomNotAvailable, "Room is not available for booking");
        }

        var isRoomApproved = await _repository.AnyAsync(iaa => iaa.RoomId == request.RoomId && iaa.Status == BookingStatus.Approved 
        && iaa.Type == BookingType.Room);
        if (isRoomApproved)
        {
            return RequestResult<bool>.Failure(ErrorCode.RoomNotAvailable, "Room is not fully empty");
        }
        var roomBookings = _repository.GetAll().Where(i => i.RoomId == request.RoomId && i.Id != request.BookingId) .ExecuteUpdate(x => x.SetProperty(y => y.Status, y => BookingStatus.Rejected)); _apartmentRepository.Get(i => i.Id == room.ApartmentId)
            .ExecuteUpdate(Apartment => Apartment.SetProperty(a => a.IsAvailable, false));


        return RequestResult<bool>.Success(true, "Room accepted!");

    }
}

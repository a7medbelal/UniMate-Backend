using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.BookingManagement;

namespace Uni_Mate.Features.BookingManagement.AcceptBooking.Commands;
public record AcceptBookRoomCommand(int BookingId, int RoomId, int ApartmentId) : IRequest<RequestResult<bool>>;

public class AcceptBookRoomCommandHandler : BaseRequestHandler<AcceptBookRoomCommand, RequestResult<bool>, Booking>
{
    private readonly IRepository<Apartment> _apartmentRepo;
    private readonly IRepository<Models.ApartmentManagement.Room> _roomRepo;
    private readonly IRepository<BookBed> _bookBedRepo;
    private readonly IRepository<BookRoom> _bookRoomRepo;

    public AcceptBookRoomCommandHandler(
        BaseRequestHandlerParameter<Booking> parameters,
        IRepository<Apartment> apartmentRepo,
        IRepository<Models.ApartmentManagement.Room> roomRepo,
        IRepository<BookBed> bookBedRepo,
        IRepository<BookRoom> bookRoomRepo
    ) : base(parameters)
    {
        _apartmentRepo = apartmentRepo;
        _roomRepo = roomRepo;
        _bookBedRepo = bookBedRepo;
        _bookRoomRepo = bookRoomRepo;
    }

    public async override Task<RequestResult<bool>> Handle(AcceptBookRoomCommand request, CancellationToken cancellationToken)
    {
        if (request.BookingId <= 0 || request.RoomId <= 0 || request.ApartmentId <= 0)
            return RequestResult<bool>.Failure(ErrorCode.InvalidData, "Invalid Booking or Room or Apartment ID");

        // bring the room from the repository of the room 
        var room = await _roomRepo.Get(rom => rom.Id==request.RoomId).Include(r => r.Beds).FirstOrDefaultAsync();
        if (room == null || !room.IsAvailable)
            return RequestResult<bool>.Failure(ErrorCode.NotAvailable, "Room is not available");

        // check on the every bed in this apartment is not available it means it booked 
        bool thereIsBedApproved =  room.Beds?.Any(b => b.IsAvailable == false)??false;
        if (thereIsBedApproved == false)
        {
            return RequestResult<bool>.Failure(ErrorCode.CanNotBook, "You Can Not Book There Is A Room Booked In The Room");
        }

        // Reject all other bookings for this room
        // Start with the bed reques for this apartment 

        #region reject the bed request for this room then reject the room request for this room and reject all booking to the apartment 


        foreach (var bed in room.Beds)
        {
            int id = bed.Id;
            await _bookBedRepo.Get(x => x.BedId == id && x.ApartmentId == request.ApartmentId && x.Type == Models.BookingManagment.BookingType.Bed)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, y => BookingStatus.Rejected));
        }
        // reject all apartment Requests
        await _repository.Get(x => x.ApartmentId == request.ApartmentId && x.Type == Models.BookingManagment.BookingType.Apartment)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, y => BookingStatus.Rejected));
       // reject all  Room Requests
        await _bookRoomRepo.Get(x => x.ApartmentId == request.ApartmentId && x.RoomId == request.RoomId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, y => BookingStatus.Rejected));
        #endregion
        // Approve selected booking
        await _repository.Get(x => x.Id == request.BookingId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, y => BookingStatus.Approved));

        room.IsAvailable = false;


        // Check if all rooms are full in the apartment
        var apartment = await _apartmentRepo.GetWithIncludeAsync(request.ApartmentId, "Rooms");
        if (apartment.Rooms.All(r => !r.IsAvailable))
        {
            apartment.IsAvailable = false;
            await _apartmentRepo.SaveIncludeAsync(apartment, nameof(Apartment.IsAvailable));

        }

        return RequestResult<bool>.Success(true, "Room booking approved successfully");
    }
}

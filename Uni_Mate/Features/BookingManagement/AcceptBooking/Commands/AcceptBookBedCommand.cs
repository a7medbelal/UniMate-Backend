using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.BookingManagement;

namespace Uni_Mate.Features.BookingManagement.AcceptBooking.Commands;
public record AcceptBookBedCommand(int BookingId, int BedId, int RoomId, int ApartmentId) : IRequest<RequestResult<bool>>;

public class AcceptBookBedCommandHandler : BaseRequestHandler<AcceptBookBedCommand, RequestResult<bool>, Booking>
{
    private readonly IRepository<Apartment> _apartmentRepo;
    private readonly IRepository<Models.ApartmentManagement.Room> _roomRepo;
    private readonly IRepository<Bed> _bedRepo;
    private readonly IRepository<BookBed> _bookBedRepo;
    private readonly IRepository<BookRoom> _bookRoomRepo;

    public AcceptBookBedCommandHandler(
        BaseRequestHandlerParameter<Booking> parameters,
        IRepository<Apartment> apartmentRepo,
        IRepository<Models.ApartmentManagement.Room> roomRepo,
        IRepository<Bed> bedRepo,
        IRepository<BookBed> bookBedRepo,
        IRepository<BookRoom> bookRoomRepo
    ) : base(parameters)
    {
        _apartmentRepo = apartmentRepo;
        _roomRepo = roomRepo;
        _bedRepo = bedRepo;
        _bookBedRepo = bookBedRepo;
        _bookRoomRepo = bookRoomRepo;
    }

    public async override Task<RequestResult<bool>> Handle(AcceptBookBedCommand request, CancellationToken cancellationToken)
    {
        if (request.BookingId <= 0 || request.BedId <= 0 || request.RoomId <= 0 || request.ApartmentId <= 0)
            return RequestResult<bool>.Failure(ErrorCode.InvalidData, "Invalid Booking/Bed/Room/Apartment ID");

        var bed = await _bedRepo.Get(x=> x.Id==request.BedId).Select(c=> new { c.Id , c.IsAvailable }).FirstOrDefaultAsync();
        if (bed == null || !bed.IsAvailable)
            return RequestResult<bool>.Failure(ErrorCode.NotAvailable, "Bed is not available");

        var newRoom = new Models.ApartmentManagement.Bed
        {
            Id = request.BedId,
            IsAvailable = false 

        };
        await _bedRepo.SaveIncludeAsync(newRoom, nameof(Bed.IsAvailable));

        

        // Check if all beds are unavailable in the room
        // To Improve 
        var room = await _roomRepo.GetWithIncludeAsync(request.RoomId, "Beds");

        var nextAvailableBed =  room.Beds?.Where(x => x.IsAvailable == true).FirstOrDefault();
        if (nextAvailableBed  == null)
        {
            // Delete All Bed Request To This Bed
            await _bookBedRepo.Get(x => x.BedId == request.BedId && x.ApartmentId == request.ApartmentId)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, y => BookingStatus.Rejected));

            room.IsAvailable = false;
            await _roomRepo.SaveIncludeAsync(room, nameof(room.IsAvailable));


            // Check If     There Is Available room
        }

        // there the approve to the bed request 
        await _repository.Get(x => x.Id == request.BookingId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, y => BookingStatus.Approved));

        // check if there a room in the apartment available 
        var apartment = await _apartmentRepo.GetWithIncludeAsync(request.ApartmentId, "Rooms");
        if (apartment.Rooms.All(r => !r.IsAvailable))
        {
            await _repository.Get(x => x.Id == request.BookingId&&x.Status != BookingStatus.Approved)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, y => BookingStatus.Approved));

            apartment.IsAvailable = false;
            await _apartmentRepo.SaveIncludeAsync(apartment, nameof(Apartment.IsAvailable));
        }

        return RequestResult<bool>.Success(true, "Bed booking approved successfully");
    }
}

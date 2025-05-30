using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Models.BookingManagment;

namespace Uni_Mate.Features.BookingManagement.AcceptBooking.Commands;
public record AcceptBookingCommand(int BookingId) : IRequest<RequestResult<bool>>;

public class  AcceptBookingCommandHandler : BaseRequestHandler<AcceptBookingCommand, RequestResult<bool>, Booking>
{
    private readonly IRepository<BookBed> _bookBedRepository;
    private readonly IRepository<BookRoom> _bookRoomRepository;
    public AcceptBookingCommandHandler(BaseRequestHandlerParameter<Booking> parameter,
        IRepository<BookRoom> bookRoomRepository,
        IRepository<BookBed> bookBedRepository
        ) : base(parameter)
    {
        _bookRoomRepository = bookRoomRepository;
        _bookBedRepository = bookBedRepository;
    }

    public override async Task<RequestResult<bool>> Handle(AcceptBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _repository.Get(x => x.Id == request.BookingId).FirstOrDefaultAsync();
        if (booking == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "booking not found.");
        }

        if(booking.Type == BookingType.Apartment)
        {
            var result = await _mediator.Send(new AcceptBookApartmentCommand(booking.Id,booking.ApartmentId));

            if(result.isSuccess)
            {
               await _repository.SaveChangesAsync();
                return RequestResult<bool>.Success(result.data,result.message);
            }
            else
            {
                return RequestResult<bool>.Failure(result.errorCode, result.message);   
            }
        }
        else if(booking.Type == BookingType.Room) 
        {
            var bookRoomRequest = await _bookRoomRepository.Get(x => x.Id == request.BookingId).Select(c=>new  { c.Id , c.RoomId , c.ApartmentId}).FirstOrDefaultAsync();
            if (bookRoomRequest == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidRequest, "The Book Room Not Exist.");
            }

            var result = await _mediator.Send(new AcceptBookRoomCommand(booking.Id, bookRoomRequest.RoomId??0, booking.ApartmentId));

            if (result.isSuccess)
            {
                await _repository.SaveChangesAsync();
                return RequestResult<bool>.Success(result.data, result.message);
            }
            else
                return RequestResult<bool>.Failure(result.errorCode, result.message);
        }
        else if( booking.Type == BookingType.Bed)
        {
            var bookBedRequest = await _bookBedRepository.Get(x => x.Id == request.BookingId).Select(c=> new { c.Id , c.ApartmentId , c.BedId , c.Bed.RoomId}).FirstOrDefaultAsync();
            if (bookBedRequest == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidRequest, "The Book Room Not Exist.");
            }

            var result = await _mediator.Send(new AcceptBookBedCommand(booking.Id, bookBedRequest.BedId,bookBedRequest.RoomId, booking.ApartmentId));

            if (result.isSuccess)
            {
                await _repository.SaveChangesAsync();
                return RequestResult<bool>.Success(result.data, result.message);
            }
            else
            {
                return RequestResult<bool>.Failure(result.errorCode, result.message);
            }

        }

          
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "The Request Can not Convert To Any Child Of The Booking.");
    }

}

using Uni_Mate.Models.ApartmentManagement;
using MediatR;
using Uni_Mate.Common.Views;
using Uni_Mate.Common.BaseHandlers;
using System.Configuration;
using Uni_Mate.Features.BookingManagement.Common;
using Uni_Mate.Features.Common;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Models.BookingManagement;

namespace Uni_Mate.Features.BookingManagement.Beds.Commands;

public record BookBedCommand(int BedId, int ApartmentId) : IRequest<RequestResult<bool>>;

public class BookBedCommandHandler : BaseRequestHandler<BookBedCommand, RequestResult<bool>, Booking>
{
    public BookBedCommandHandler(BaseRequestHandlerParameter<Booking> parameters) : base(parameters)
    {
    }

    public async override Task<RequestResult<bool>> Handle(BookBedCommand request, CancellationToken cancellationToken)
    {
        //var isUserExistQuery = new IsUserExistQuery(_userInfo.ID);
        var isUserExistQuery = new IsUserExistQuery("2");
        
        var result = await _mediator.Send(isUserExistQuery, cancellationToken);
        if (!result.isSuccess)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "can't process on this user");
        }

        var isBedExistQuery = new IsBedExistQuery(request.BedId);
        var bedResult = await _mediator.Send(isBedExistQuery, cancellationToken);
        if (!bedResult.isSuccess)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "Bed not found");
        }

        var BookBeforeQuery = new BookBeforeQuery("2", request.ApartmentId);
        result = await _mediator.Send(BookBeforeQuery, cancellationToken);
        if (!result.isSuccess)
        {
            return RequestResult<bool>.Failure(ErrorCode.AlreadyExists, "User already booked a bed in this apartment");
        }

        Booking booking = new()
        {
            ApartmentId = request.ApartmentId,
            StudentId = _userInfo.ID,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1),
        };
        BookBed bookBed = new()
        {
            BookingId = booking.Id,
            BedId = request.BedId,
            StudentId = _userInfo.ID,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate
        };
        await _repository.Add(booking);
        await _repository.Add(bookBed);
        await _repository.SaveChangesAsync();
        
        return RequestResult<bool>.Success(true, "Bed booked successfully");
    }
}
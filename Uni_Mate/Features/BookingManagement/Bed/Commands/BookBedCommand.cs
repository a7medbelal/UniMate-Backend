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

public class BookBedCommandHandler : BaseRequestHandler<BookBedCommand, RequestResult<bool>, BookBed>
{
    public BookBedCommandHandler(BaseRequestHandlerParameter<BookBed> parameters) : base(parameters)
    {
    }

    public async override Task<RequestResult<bool>> Handle(BookBedCommand request, CancellationToken cancellationToken)
    {
        var BookBeforeQuery = new BookBeforeQuery("2", request.ApartmentId);
        var result = await _mediator.Send(BookBeforeQuery, cancellationToken);
        if (!result.isSuccess)
        {
            return RequestResult<bool>.Failure(ErrorCode.AlreadyExists, "User already booked a bed in this apartment");
        }

        BookBed bookBed = new()
        {
            ApartmentId = request.ApartmentId,
            StudentId = "2",
            BedId = request.BedId,
            Status = BookingStatus.Pending,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1),
        };
        await _repository.Add(bookBed);
        await _repository.SaveChangesAsync();
        
        return RequestResult<bool>.Success(true, "Bed booked successfully");
    }
}
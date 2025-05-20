using Uni_Mate.Models.ApartmentManagement;
using MediatR;
using Uni_Mate.Common.Views;
using Uni_Mate.Common.BaseHandlers;
using System.Configuration;

namespace Uni_Mate.Features.BookingManagement.Beds.Commands;

public record BookBedCommand(int BedId, int ApartmentId, int RoomId) : IRequest<RequestResult<bool>>;

public class BookBedCommandHandler : BaseRequestHandler<BookBedCommand, RequestResult<bool>, Bed>
{
    public BookBedCommandHandler(BaseRequestHandlerParameter<Bed> parameters) : base(parameters)
    {
    }

    public async override Task<RequestResult<bool>> Handle(BookBedCommand request, CancellationToken cancellationToken)
    {
        
        throw new NotImplementedException();
    }
}
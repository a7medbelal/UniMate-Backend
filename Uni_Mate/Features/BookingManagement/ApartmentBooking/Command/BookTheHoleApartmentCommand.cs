using MediatR;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.BookingManagement.ApartmentBooking.Command
{
    public record BookTheHoleApartmentCommand(int ApartmentID) : IRequest<RequestResult<bool>>;
    public class BookTheHoleApartmentCommandHandler : BaseRequestHandler<BookTheHoleApartmentCommand, RequestResult<bool>, Apartment>

    {       
        public BookTheHoleApartmentCommandHandler(BaseRequestHandlerParameter<Apartment> parameters) : base(parameters)
        {

        }

        public override Task<RequestResult<bool>> Handle(BookTheHoleApartmentCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }
}
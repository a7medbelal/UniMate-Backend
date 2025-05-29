using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.BookingManagement;

namespace Uni_Mate.Features.BookingManagement.AcceptBooking.Commands;
public record AcceptBookApartmentCommand(int BookingId, int ApartmentId) : IRequest<RequestResult<bool>>;

public class AcceptBookApartmentCommandHandler : BaseRequestHandler<AcceptBookApartmentCommand, RequestResult<bool>, Booking>
{
    private readonly IRepository<Apartment> apartmentRepository;
    public AcceptBookApartmentCommandHandler(BaseRequestHandlerParameter<Booking> parameters, IRepository<Apartment> _apartmentRepository) : base(parameters)
    {
        apartmentRepository = _apartmentRepository;
    }

    public async override Task<RequestResult<bool>> Handle(AcceptBookApartmentCommand request, CancellationToken cancellationToken)
    {
        var apartment = await apartmentRepository.Get(i => i.Id == request.ApartmentId).FirstOrDefaultAsync();
        if (apartment == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found");
        }
        if (apartment.IsAvailable == false)
        {
            return RequestResult<bool>.Failure(ErrorCode.ApartmentNotAvailable, "Apartment is not available for booking");
        }

        var isApartmentApproved = await _repository.AnyAsync(iaa => iaa.ApartmentId == request.ApartmentId && iaa.Status == BookingStatus.Approved);
        if (isApartmentApproved)
        {
            return RequestResult<bool>.Failure(ErrorCode.ApartmentNotAvailable, "Apartment is not fully empty");
        }

        var Bookings = _repository.GetAll().Where(i => i.ApartmentId == request.ApartmentId && i.Id != request.BookingId)
            .ExecuteUpdate(x => x.SetProperty(y => y.Status, y => BookingStatus.Rejected));

        var acceptedBooking = _repository.Get(ab => ab.Id == request.ApartmentId)
            .ExecuteUpdate(x => x.SetProperty(xx => xx.Status, BookingStatus.Approved));

        return RequestResult<bool>.Success(true , "Apartment accepted!");
    }
}

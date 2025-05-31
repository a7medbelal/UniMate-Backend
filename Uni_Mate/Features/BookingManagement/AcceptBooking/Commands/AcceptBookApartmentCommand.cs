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
        /*
         * book the entire apartment  :
                     must check that the apartment there is not even a single be booked and make sure also that it is valide 
                     then then clancle all king of the apartment 
                     then make the apartment as invalide to make the to make sure that no one can else book this apartment 
         */
        if (request.BookingId <=0 || request.ApartmentId <=0) 
            {
                return RequestResult<bool>.Failure(ErrorCode.NotValide, "The Booking Id And The Apartment Id are Not Valid .");
            }

        var apartment = await apartmentRepository.Get(i => i.Id == request.ApartmentId).Select(c=> new  { c.Id , c.IsAvailable}).FirstOrDefaultAsync();

        if (apartment == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found");
        }
        if (apartment.IsAvailable == false)
        {
            return RequestResult<bool>.Failure(ErrorCode.ApartmentNotAvailable, "Apartment is not available for booking");
        }

        //var isApartmentApproved = await _repository.AnyAsync(iaa => iaa.ApartmentId == request.ApartmentId && iaa.Status == BookingStatus.Approved);
        //if (isApartmentApproved)
        //{
        //    return RequestResult<bool>.Failure(ErrorCode.ApartmentNotAvailable, "You Can Not Book The Entire Apartment.");
        //}
        var Booking = await _repository.Get(i => i.Id == request.BookingId).Select(c=> new  { c.Id , c.Status}).FirstOrDefaultAsync();



        var Bookings = _repository.Get(ab => ab.Id == request.BookingId && ab.Id != request.BookingId)
            .ExecuteUpdate(x => x.SetProperty(y => y.Status, y => BookingStatus.Rejected));

        var acceptedBooking = _repository.Get(ab => ab.Id == request.ApartmentId)
            .ExecuteUpdate(x => x.SetProperty(xx => xx.Status, BookingStatus.Approved));

        var newAparmtent = new Apartment { Id = request.ApartmentId, IsAvailable = false }; 

        var updateApartment = await apartmentRepository.SaveIncludeAsync(newAparmtent, nameof(newAparmtent.IsAvailable));

      
        if(!updateApartment)
        {
            return RequestResult<bool>.Failure(ErrorCode.SaveFailed, "Saving The Status Of The Apartment Failed");
        }
        return RequestResult<bool>.Success(true , "Apartment accepted!");
    }
}

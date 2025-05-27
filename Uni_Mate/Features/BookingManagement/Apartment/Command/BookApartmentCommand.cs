using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Features.Common;
using Uni_Mate.Models.BookingManagement;

namespace Uni_Mate.Features.BookingManagement.Apartments.Command
{
    public record BookApartmentCommand(int ApartmentId) : IRequest<RequestResult<bool>>;

    public class BookApartmentHandler : BaseRequestHandler<BookApartmentCommand, RequestResult<bool>,BookApartment>
    {

        private readonly IRepository<Models.ApartmentManagement.Apartment> _apartmentRepository;
        public BookApartmentHandler(BaseRequestHandlerParameter<BookApartment> parameter,
            IRepository<Models.ApartmentManagement.Apartment> apartmentRepository) : base(parameter)
        {
            _apartmentRepository = apartmentRepository;
        }
        public override async Task<RequestResult<bool>> Handle(BookApartmentCommand request, CancellationToken cancellationToken)
        {

            /*
             * 1 - Check if the user exists
             * 2 - Check if the apartment exists and is available
             * 3 - Check if the user has already booked a bed in this apartment or a room
             * 4 - Check if there is some bed or room is invalide (means it been booked so you can't book the entire apartment)
             * 5 - Book the apartment for the user
             */


            // Check if the user exists
            var userId = _userInfo.ID;
            if (string.IsNullOrEmpty(userId) || userId == "-1")
            {
                return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");
            }

            // Check if the user exists
            var userCheck = await _mediator.Send(new IsUserExistQuery(userId));
            if (!userCheck.isSuccess)
            {
                return RequestResult<bool>.Failure(userCheck.errorCode, "User not found");
            }

            // Check if the apartment exists 
            if (request.ApartmentId <= 0)
            {
                return RequestResult<bool>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found");
            }

            var apartment = await _apartmentRepository.GetWithIncludeAsync(request.ApartmentId, "Rooms.Beds");

            if (apartment == null)
                return RequestResult<bool>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found");

            if (!apartment.IsAvailable)
                return RequestResult<bool>.Failure(ErrorCode.Booked, "Apartment is already booked or unavailable");


            bool anyRoomUnavailable = apartment.Rooms?.Any(r => !r.IsAvailable) ?? false;
            bool anyBedUnavailable = apartment.Rooms?
                .SelectMany(r => r.Beds)
                .Any(b => !b.IsAvailable) ?? false;

            if (anyRoomUnavailable || anyBedUnavailable)
            {
                return RequestResult<bool>.Failure(ErrorCode.Booked, "Cannot book entire apartment: a room or bed is already reserved.");
            }

            // Check if the user has already booked a bed in this apartment or a room 
            var BookBeforeQuery = new Common.BookBeforeQuery(userId, request.ApartmentId);
            var result = await _mediator.Send(BookBeforeQuery);
            if (!result.isSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.AlreadyExists,"User Already Booked room or bed in the apartment");
            }


            // finally Halal alaik el Apartment 
            var booking = new BookApartment
            {
                ApartmentId = request.ApartmentId,
                StudentId = userId,
            };

            await _repository.AddAsync(booking);
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Apartment booked successfully");

        }
    }
}

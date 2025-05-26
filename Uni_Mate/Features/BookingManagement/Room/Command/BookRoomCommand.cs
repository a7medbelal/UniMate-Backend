using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Features.BookingManagement.Common;
using Uni_Mate.Features.Common;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Models.BookingManagment;

namespace Uni_Mate.Features.BookingManagement.Room.Command
{
    public record BookRoomCommand(int ApartmentId, int RoomId) : IRequest<RequestResult<bool>>;

    public class BookRoomCommandHandler : BaseRequestHandler<BookRoomCommand, RequestResult<bool>, BookRoom>
    {
        private readonly IRepository<Models.ApartmentManagement.Room> _roomRepository;
        public BookRoomCommandHandler(BaseRequestHandlerParameter<BookRoom> parameters, IRepository<Models.ApartmentManagement.Room> roomRepository) : base(parameters)
        {
            _roomRepository = roomRepository;
        }
        public async override Task<RequestResult<bool>> Handle(BookRoomCommand request, CancellationToken cancellationToken)
        {
            /*
             * 1 - Check if the user exists
             * 2 - Check if the apartment exists and is valid
             * 3 - Check if the room exists and belongs to the specified apartment
             * 4 - Check if has invalide bed in the room (bed Booked before)
             */

            // Check if the user exists
            var userId = _userInfo.ID;
            if (string.IsNullOrEmpty(userId) || userId == "-1")
            {
                return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");
            }

            var userCheck = await _mediator.Send(new IsUserExistQuery(userId));
            if (!userCheck.isSuccess)
            {
                return RequestResult<bool>.Failure(userCheck.errorCode, "User not found");
            }

            // Check if the apartment exists and  valid
            if (request.ApartmentId <= 0)
            {
                return RequestResult<bool>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found");
            }
            var apartmentCheck = await _mediator.Send(new IsValideApartmentQuery(request.ApartmentId));
            if (!apartmentCheck.isSuccess)
            {
                return RequestResult<bool>.Failure(apartmentCheck.errorCode, apartmentCheck.message);
            }
            var BookBeforeQuery = new BookBeforeQuery(_userInfo.ID, request.ApartmentId);
            var result = await _mediator.Send(BookBeforeQuery, cancellationToken);
            if (!result.isSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.AlreadyExists,result.message);
            }

            var room = await _roomRepository.GetWithIncludeAsync(request.RoomId, "Beds");
            if (room == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Room not found");
            }

            // Check if the room belongs to the specified apartment
            if (room.ApartmentId != request.ApartmentId)
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidData, "Room does not belong to this apartment");
            }

            //Check if there is even a single bed taken to make the booking for the room not valid #__#
            var bed = room.Beds?.FirstOrDefault(b => b.IsAvailable == false);

            if (bed != null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotValide, "There is at least bed booked before you can't book this room");
            }

            BookRoom bookRoom = new()
            {
                ApartmentId = request.ApartmentId,
                StudentId = _userInfo.ID,
                RoomId = request.RoomId,
                Type = BookingType.Room,
            };
            await _repository.Add(bookRoom);
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Bed booked successfully");
        }
    }
}

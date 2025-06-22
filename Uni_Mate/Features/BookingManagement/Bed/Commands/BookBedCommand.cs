using MediatR;
using Uni_Mate.Common.Views;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Features.BookingManagement.Common;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Features.Common;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.BookingManagment;
using Uni_Mate.Features.Notifiaction.NottifcationForBooking;

namespace Uni_Mate.Features.BookingManagement.Beds.Commands;

public record BookBedCommand(int RoomId, int ApartmentId) : IRequest<RequestResult<bool>>;

public class BookBedCommandHandler : BaseRequestHandler<BookBedCommand, RequestResult<bool>, BookBed>
{
    private readonly IRepository<Models.ApartmentManagement.Room> _roomRepository;
    public BookBedCommandHandler(BaseRequestHandlerParameter<BookBed> parameters,
        IRepository<Models.ApartmentManagement.Room> roomRepository) : base(parameters)
    {
        _roomRepository = roomRepository;
    }

    public async override Task<RequestResult<bool>> Handle(BookBedCommand request, CancellationToken cancellationToken)
    {
        // ya hossam lo tabitak haktilak   ^--^

        /*
        * 1 - check on the Existance of the user 
        * 2 - check on the validation of apartment 
        * 3 - check on the validation of Room 
        * 4 - check on the validation of Bed 
        * 5 - book the most valide bed in the room 
        */

        // Check if the user exists ya belel
        var userId = _userInfo.ID;
        if(string.IsNullOrEmpty(userId) || userId == "-1")
        {
            return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");
        }
        var userCheck = await _mediator.Send(new IsUserExistQuery(userId));
        if (!userCheck.isSuccess)
        {
            return RequestResult<bool>.Failure(userCheck.errorCode, "User not found");
        }


        // Check if the apartment exists And the Validation of the apartment
        if (request.ApartmentId <= 0)
        {
            return RequestResult<bool>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found");
        }

        var apartmentCheck = await _mediator.Send(new IsValideApartmentQuery(request.ApartmentId));
        if (!apartmentCheck.isSuccess)
        {
            return RequestResult<bool>.Failure(apartmentCheck.errorCode,apartmentCheck.message);
        }

        // if the Student booked something before in the apartment 
        var BookBeforeQuery = new BookBeforeQuery(_userInfo.ID, request.ApartmentId);
        var result = await _mediator.Send(BookBeforeQuery, cancellationToken);
        if (!result.isSuccess)
        {
            return RequestResult<bool>.Failure(ErrorCode.AlreadyExists, result.message);
        }

        var room = await _roomRepository.GetWithIncludeAsync(request.RoomId,"Beds");
        if (room == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "Room not found");
        }
        // check if the room is not available
        if (!room.IsAvailable)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotValide, "Room is not available");
        }

        // Check if the room belongs to the specified apartment
        if (room.ApartmentId != request.ApartmentId)
        {
            return RequestResult<bool>.Failure(ErrorCode.InvalidData, "Room does not belong to this apartment");
        }

        var bed = room.Beds?.FirstOrDefault(b => b.IsAvailable == true);

        if (bed == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotValide, "No available beds in this room");
        }

        BookBed bookBed = new()
        {
            ApartmentId = request.ApartmentId,
            StudentId = _userInfo.ID,
            BedId = bed.Id,
            Type = BookingType.Bed,
        };
        await _repository.Add(bookBed);
        await _repository.SaveChangesAsync();

        //publis an event here to send email to owner
        await _mediator.Publish(new BookingAccepteNotification(bookBed.Id ,userCheck.data , bookBed.Type ,  apartmentCheck.data ,bookBed.CreatedDate)); 
        
        return RequestResult<bool>.Success(true, "Bed booked successfully");
    }
}
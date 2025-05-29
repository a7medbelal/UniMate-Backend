using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Models.BookingManagment;

namespace Uni_Mate.Features.BookingManagement.AcceptBooking.Commands;
public record AcceptBookBedCommand() : IRequest<RequestResult<bool>>;

public class AcceptBookBedCommandHandler : BaseRequestHandler<AcceptBookBedCommand, RequestResult<bool>, BookBed>
{
    private readonly IRepository<Uni_Mate.Models.ApartmentManagement.Room> _roomRepository;
    private readonly IRepository<Apartment> _apartmentRepository;
    private readonly IRepository<Bed> _bedRepository;
    public AcceptBookBedCommandHandler(BaseRequestHandlerParameter<BookBed> parameter, IRepository<Bed> repository, 
        IRepository<Uni_Mate.Models.ApartmentManagement.Room> roomRepository,
        IRepository<Apartment> apartmentRepository) : base(parameter)
    {
        _apartmentRepository = apartmentRepository;
        _roomRepository = roomRepository;
        _bedRepository = repository;
    }
    public async override Task<RequestResult<bool>> Handle(AcceptBookBedCommand request, CancellationToken cancellationToken)
    {
        var bed = _bedRepository.GetAll().Where(b => b.IsAvailable == true).FirstOrDefault();
        if (bed == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "No available bed found");
        }
        
        if (bed.IsAvailable == false)
        {
            return RequestResult<bool>.Failure(ErrorCode.BedNotAvailable, "Bed is not available for booking");
        }

        var isBedApproved = await _repository.AnyAsync(iaa => iaa.BedId == bed.Id && iaa.Status == BookingStatus.Approved && iaa.Type == BookingType.Bed);
        if (isBedApproved)
        {
            return RequestResult<bool>.Failure(ErrorCode.BedNotAvailable, "Bed is not fully empty");
        }

        var bedBookings = _repository.GetAll().Where(i => i.BedId == bed.Id)
            .ExecuteUpdate(x => x.SetProperty(y => y.Status, y => BookingStatus.Rejected));

        return RequestResult<bool>.Success(true, "Beed booking accepted!");
    }
}

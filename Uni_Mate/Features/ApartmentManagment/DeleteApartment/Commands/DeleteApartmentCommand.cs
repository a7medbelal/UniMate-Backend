using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.DeleteApartment.Commands;
public record DeleteApartmentCommand(int ApartmentId) : IRequest<RequestResult<bool>>;

public class DeleteApartmentCommandHandler : BaseRequestHandler<DeleteApartmentCommand, RequestResult<bool>, Apartment>
{
    public DeleteApartmentCommandHandler(BaseRequestHandlerParameter<Apartment> parameters) : base(parameters)
    {
    }
    public override async Task<RequestResult<bool>> Handle(DeleteApartmentCommand request, CancellationToken cancellationToken)
    {
        Apartment apartment = await _repository.GetByIDAsync(request.ApartmentId);
        if (apartment == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "Apartment not found");
        }
        var deleteApartmentImagesCommand = new DeleteApartmentImagesCommand(request.ApartmentId);
        var result = await _mediator.Send(deleteApartmentImagesCommand, cancellationToken);
        if (!result.isSuccess)
        {
            return RequestResult<bool>.Failure(ErrorCode.DeletionFailed, "Failed to delete apartment");
        }
        await _repository.HardDeleteAsync(apartment);
        await _repository.SaveChangesAsync();

        return RequestResult<bool>.Success(true, "Apartment deleted successfully");
    }
}

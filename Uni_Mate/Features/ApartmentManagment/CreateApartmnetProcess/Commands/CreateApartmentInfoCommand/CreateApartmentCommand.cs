using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CreateApartmentInfoCommand
{
    public record CreateApartmentCommand(string OwnerID,
        int Num,
        string Location,
        string? Description,
        int Capecity,
        int NumberOfRooms,
        string? DescripeLocation,
        string Floor,
        Gender GenderAcceptance,
        ApartmentDurationType DurationType
    ) : IRequest<RequestResult<int>>;

    public class CreateApartmentCommandHandler : BaseRequestHandler<CreateApartmentCommand, RequestResult<int>, Apartment>
    {
        public CreateApartmentCommandHandler(BaseRequestHandlerParameter<Apartment> parameters) : base(parameters) { }

        public override async Task<RequestResult<int>> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
        {
            var apartment = new Apartment
            {
                Num = request.Num,
                Location = request.Location,
                Description = request.Description,
                DescripeLocation = request.DescripeLocation,
                Gender = request.GenderAcceptance,
                Floor = request.Floor,
                DurationType = request.DurationType,
                OwnerID = request.OwnerID,
                Capecity = request.Capecity,
                NumberOfRooms = request.NumberOfRooms,
            };

            await _repository.AddAsync(apartment);
            await _repository.SaveChangesAsync();
            return RequestResult<int>.Success(apartment.Id, "Apartment created successfully");
        }
    }
}

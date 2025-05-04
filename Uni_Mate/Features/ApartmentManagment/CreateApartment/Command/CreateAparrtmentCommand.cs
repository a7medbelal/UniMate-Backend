using MediatR;
using System.Threading;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartment.Command
{
    public record CreateApartmentCommand(
        int Num , 
        string Location,
        string Description,
        string DescripeLocation,
        string Floor,
        Gender GenderAcceptance,
        ApartmentDurationType DurationType
    ) : IRequest<RequestResult<int>>;

    public class CreateApartmentCommandHandler : BaseRequestHandler<CreateApartmentCommand, RequestResult<int>, Apartment>
    {
        public CreateApartmentCommandHandler(BaseRequestHandlerParameter<Apartment> parameters) : base(parameters) { }

        public override async Task<RequestResult<int>> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
        {
            var ownerID = _userInfo.ID;
            if (String.IsNullOrEmpty(ownerID))
                return RequestResult<int>.Failure( ErrorCode.OwnerNotAuthried , "Owner Not Authrized");

            var apartmentExist = await _repository.AnyAsync(x => x.Num == request.Num && x.OwnerID == ownerID);
            if (apartmentExist)
                return RequestResult<int>.Failure(ErrorCode.ApartmentAlreadyExist, "Apartment already exist");


            var apartment = new Apartment
            { 
               Num = request.Num,   
               Location = request.Location,
               Description = request.Description,
               DescripeLocation = request.DescripeLocation,
               Gender = request.GenderAcceptance,
               Floor = request.Floor,  
               DurationType = request.DurationType,
               OwnerID =  ownerID,
            };

            await _repository.AddAsync(apartment);
            await _repository.SaveChangesAsync();
            return RequestResult<int>.Success(apartment.Id, "Apartment created successfully");
        }
    }

}

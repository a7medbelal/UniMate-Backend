﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.AddRoomWithBedsCommands;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommands;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CreateApartmentInfoCommand;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyImages;
using Uni_Mate.Features.Common.ApartmentManagement.UploadApartmentCommand;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CreateFullApartmentOrcasterartor
{
    public record SubmitPostCommand(
        Location Location,
        string? Description,
        string? DescribeLocation,
        string Floor,
        int Capecity,  
        Gender GenderAcceptance,
        ApartmentDurationType DurationType,
        List<RoomBedViewModel> Rooms,
        List<FacilityApartmentViewModel> CategoryFacilities,
		UploadApartmentImagesViewModel Images    
        ) : IRequest<RequestResult<bool>>;

    public class SubmitPostCommandHandler : BaseRequestHandler<SubmitPostCommand, RequestResult<bool>, Apartment>
    {
        public SubmitPostCommandHandler(BaseRequestHandlerParameter<Apartment> parameters)
            : base(parameters) { }

        public override async Task<RequestResult<bool>> Handle(SubmitPostCommand request, CancellationToken cancellationToken)
        {
            var ownerID = _userInfo.ID;

            if (string.IsNullOrEmpty(ownerID))
                return RequestResult<bool>.Failure(ErrorCode.OwnerNotAuthorized, "Owner Not Authorized");

            //var apartmentExist = await _repository.AnyAsync(x => x.OwnerID == ownerID);
            //if (apartmentExist)
            //    return RequestResult<bool>.Failure(ErrorCode.ApartmentAlreadyExist, "Apartment already exists");

            var newApartment = await _mediator.Send(new CreateApartmentCommand(
                ownerID,
                request.Location,
                request.Description,
                request.Capecity,
                request.DescribeLocation,
                request.Floor,
                request.GenderAcceptance,
                request.DurationType 
            ));

            if (!newApartment.isSuccess)
                return RequestResult<bool>.Failure(newApartment.errorCode, newApartment.message);

            var addRooms = await _mediator.Send(new AddRoomCommand( request.Rooms, newApartment.data));
            if (!addRooms.isSuccess)
                return RequestResult<bool>.Failure(addRooms.errorCode, addRooms.message);

            var addFacilities = await _mediator.Send(new CategoryWithFaciltiesCommand(request.CategoryFacilities, newApartment.data));
            if (!addFacilities.isSuccess)
                return RequestResult<bool>.Failure(addFacilities.errorCode, addFacilities.message);

            var uploadImages = await _mediator.Send(new UploadApartmentImagesCommand(newApartment.data , request.Images));
            if (!uploadImages.isSuccess)
                return RequestResult<bool>.Failure(uploadImages.errorCode, uploadImages.message);

            #region SetSome Information toApatrtment

            //need to set the price for the apartment and number of rooms and beds
            // Assuming you want to set the price from the first room 
            var price = addRooms.data.Price;
            var NumberOfRooms = addRooms.data.NumberOfRooms;
            var NumberOfBeds = addRooms.data.NumberOfBeds;
            if (price  >  0)
            {
                var apartmentMoreInfo = new Apartment
                {
                    Id = newApartment.data,
                    Price = price,
                    NumberOfRooms = NumberOfRooms,
                    Capecity = addRooms.data.NumberOfBeds,  

                };
                await _repository.SaveIncludeAsync(apartmentMoreInfo, nameof(apartmentMoreInfo.Price) , nameof(apartmentMoreInfo.NumberOfRooms), nameof(apartmentMoreInfo.Capecity));
            }   

            #endregion

            await _repository.SaveChangesAsync(); 
            return RequestResult<bool>.Success(true);
        }
    }

}

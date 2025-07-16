
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Threading;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Features.Common.ApartmentManagement.ApartmerntDTO;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.OwnerManager.GetApartmentOwners.Queries;
public record GetApartmentsOwnerQuery() : IRequest<RequestResult<List<GetApartmentDTO>>>;

public class GetApartmentsOwnerQueryHandler : BaseRequestHandler<GetApartmentsOwnerQuery, RequestResult<List<GetApartmentDTO>>, Apartment>
{
    IRepository<ApartmentFacility> _apartmentFacilityRepository;
    IRepository<Room> _roomRepository;
    IRepository<Image> _imageRepository;


    public GetApartmentsOwnerQueryHandler(BaseRequestHandlerParameter<Apartment> parameters, IRepository<Room> roomRepository, IRepository<Image> imageRepository) : base(parameters)
    {
        _roomRepository = roomRepository;
        _imageRepository = imageRepository;
    }
    public override async Task<RequestResult<List<GetApartmentDTO>> > Handle(GetApartmentsOwnerQuery request, CancellationToken cancellationToken)
    {
         var userId = _userInfo.ID;
        var query =await _repository.Get(c=> c.OwnerID == userId ) 
            .Select(x => new GetApartmentDTO
            {
                Id = x.Id,
                Location = x.Location,
                Gender = x.Gender.ToString(),
                Floor = x.Floor ?? "unknown",
                DetailedAddress = x.DescripeLocation ?? "unknown",
                OwnerName = x.Owner != null ? x.Owner.Fname + " " + x.Owner.Lname : string.Empty,
                NumberOfRooms = x.NumberOfRooms,
                Price = x.Price,
            }).OrderBy(x => x.Id).ToListAsync(); 


        var apartmentIds = query.Select(a => a.Id).ToList();
        var imageTask = await _imageRepository.Get(i => apartmentIds.Contains(i.ApartmentId))
            .Select(i => new { i.ApartmentId, i.ImageUrl })
            .ToListAsync();
            

        var imagesGrouped = imageTask
            .GroupBy(i => i.ApartmentId)
            .ToDictionary(g=> g.Key, g => g.Select(i => i.ImageUrl).ToList());

        foreach (var apartment in query)
        {
            apartment.Images = imagesGrouped.TryGetValue(apartment.Id, out var imgs)
                ? imgs
                : new List<string>();
        }



        return RequestResult<List<GetApartmentDTO>>.Success( query , "owner apartments");
    }


}




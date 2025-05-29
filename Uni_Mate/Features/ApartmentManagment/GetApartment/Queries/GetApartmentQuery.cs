using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.GetApartment.Queries;
public record GetApartmentQuery(int PageNumber, int PageSize) : IRequest<RequestResult<Pagination<GetApartmentDTO>>>;

public class GetApartmentQueryHandler : BaseRequestHandler<GetApartmentQuery, RequestResult<Pagination<GetApartmentDTO>>, Apartment>
{
    IRepository<ApartmentFacility> _apartmentFacilityRepository;
    IRepository<Room> _roomRepository;
    IRepository<Image> _imageRepository;


    public GetApartmentQueryHandler(BaseRequestHandlerParameter<Apartment> parameters, IRepository<Room> roomRepository, IRepository<Image> imageRepository) : base(parameters)
    {
        _roomRepository = roomRepository;
        _imageRepository = imageRepository;
    }
    public override async Task<RequestResult<Pagination<GetApartmentDTO>>> Handle(GetApartmentQuery request, CancellationToken cancellationToken)
    {
        // Step 1: Get paginated apartment IDs and core data
        var apartmentQuery = _repository.GetAll()
            .Where(a => !a.Deleted)
            .Select(a => new
            {
                a.Id,
                a.Floor,
                a.Location,
                a.Gender,
                OwnerName = (a.Owner.Fname ?? "") + " " + (a.Owner.Lname ?? "")
            });

        var pagedApartments = await Pagination<dynamic>.ToPagedList(
            apartmentQuery,
            request.PageNumber,
            request.PageSize
        );

        var apartmentIds = pagedApartments.Select(a => a.Id).ToList();

        // Step 2: Batch retrieve related data in parallel
        var imagesTask = _imageRepository.GetAll()
            .Where(i => apartmentIds.Contains(i.ApartmentId))
            .GroupBy(i => i.ApartmentId)
            .ToDictionaryAsync(g => g.Key, g => g.Select(i => i.ImageUrl).ToList(), cancellationToken);

        var roomsTask = _roomRepository.GetAll()
            .Where(r => apartmentIds.Contains(r.ApartmentId))
            .GroupBy(r => r.ApartmentId)
            .ToDictionaryAsync(
                g => g.Key,
                g => new {
                    Count = g.Count(),
                    MinPrice = g.Min(r => r.Price)
                },
                cancellationToken
            );

        var facilitiesTask = _apartmentFacilityRepository.GetAll()
            .Where(af => apartmentIds.Contains(af.ApartmentId))
            .Select(af => new { af.ApartmentId, af.Facility.Name })
            .GroupBy(af => af.ApartmentId)
            .ToDictionaryAsync(g => g.Key, g => g.Select(x => x.Name).ToList(), cancellationToken);

        await Task.WhenAll(imagesTask, roomsTask, facilitiesTask);
        var imagesDict = await imagesTask;
        var roomsDict = await roomsTask;
        var facilitiesDict = await facilitiesTask;

        // Step 3: Compose DTOs
        var apartmentDTOs = pagedApartments.Select(a => new GetApartmentDTO
        {
            Floor = a.Floor ?? "unknown",
            Address = a.Location.ToString(), // Adjust if Location is complex object
            Gender = a.Gender.ToString(),
            OwnerName = a.OwnerName,
            //Images =imagesDict.GetValueOrDefault(a.Id, new List<string>()),
            //Facilities = facilitiesDict.GetValueOrDefault(a.Id, new List<string>()),
            //umberOfRooms = roomsDict.TryGetValue(a.Id, out var roomInfo) ? roomInfo.Count : 0,
            //Price = roomsDict.TryGetValue(a.Id, out roomInfo) ? roomInfo.MinPrice : 0
        }).ToList();

        // Step 4: Return paginated result
        var result = new Pagination<GetApartmentDTO>(
            apartmentDTOs,
            pagedApartments.TotalCount,
            pagedApartments.CurrentPage,
            pagedApartments.pageSize
        );

        return RequestResult<Pagination<GetApartmentDTO>>.Success(result, "Pagination succeeded");
    }








    /**
     * TO DO:
     * add favourite in the future once implemented
    // */
    //var query = _repository.GetAll()
    //    .Select(x => new GetApartmentDTO
    //    {
    //        Images = (List<string>)x.Images.Select(i => i.ImageUrl),
    //        Address = nameof(x.Location),
    //        Gender = nameof(x.Gender),
    //        Floor = x.Floor?? "unknown",
    //        OwnerName =  (x.Owner.Fname + " " + x.Owner.Lname),
    //        NumberOfRooms = x.Rooms != null ? x.Rooms.Count() : 0,
    //        Facilities = x.ApartmentFacilities.Select(f => f.Facility.Name).ToList(),
    //        Price = x.Rooms != null && x.Rooms.Any() ? x.Rooms.First().Price : 0
    //    });

    //var apartemtent = _repository.GetAll()
    //  .Select(a => new
    //   {
    //     a.Id,
    //     a.Floor,
    //     OwnerName = a.Owner.Fname + " " + a.Owner.Lname
    //   });

    //var pagedBase = await Pagination<dynamic>.ToPagedList(apartemtent, request.PageNumber, request.PageSize);

    //var apartmentIds = pagedBase.Select(a => a.Id).ToList(); // Safe now — strongly typed int
    //// Step 3: Fetch related data in batches to avoid N+1 problems
    //var images = await _imageRepository.GetAll()
    //    .Where(i => apartmentIds.Contains(i.ApartmentId))
    //    .GroupBy(i => i.ApartmentId)
    //    .ToDictionaryAsync(g => g.Key, g => g.Select(i => i.ImageUrl).ToList());

    //var rooms = await _roomRepository.GetAll()
    //    .Where(r => apartmentIds.Contains(r.ApartmentId))
    //    .GroupBy(r => r.ApartmentId)
    //    .ToDictionaryAsync(g => g.Key, g => g.ToList(), cancellationToken);

    //var facilities = await _apartmentFacilityRepository.GetAll()
    //    .Where(af => apartmentIds.Contains(af.ApartmentId))
    //    .Select(af => new { af.ApartmentId, af.Facility.Name })
    //    .GroupBy(x => x.ApartmentId)
    //    .ToDictionaryAsync(g => g.Key, g => g.Select(x => x.Name).ToList(), cancellationToken);



    //// Step 4: Final projection into DTOs
    //var apartmentDTOs = pagedBase.Select(a =>
    //    new GetApartmentDTO
    //    {
    //        Floor = a.Floor ?? "unknown",
    //        Address = a.Location,
    //        Gender = a.Gender,
    //        OwnerName = a.OwnerName,
    //        Images = images.ContainsKey(a.Id) ? images[a.Id] : new List<string>(),
    //        Facilities = facilities.ContainsKey(a.Id) ? facilities[a.Id] : new List<string>(),
    //        NumberOfRooms = rooms.ContainsKey(a.Id) ? rooms[a.Id].Count : 0,
    //        Price = rooms.ContainsKey(a.Id) && rooms[a.Id].Any() ? rooms[a.Id].First().Price : 0
    //    }).ToList();

    //// Step 5: Return paginated result with updated DTOs
    //var finalPagination = new Pagination<GetApartmentDTO>(
    //    apartmentDTOs,
    //    pagedBase.TotalCount,
    //    pagedBase.CurrentPage,
    //    pagedBase.pageSize
    //);

    ////return RequestResult<Pagination<GetApartmentDTO>>.Success(finalPagination, "Pagination) Worked");


    //var paginatedResult = await Pagination<GetApartmentDTO>.ToPagedList(query, request.PageNumber, request.PageSize);

    //return RequestResult<Pagination<GetApartmentDTO>>.Success(paginatedResult, "Pagination Worked");
}
 
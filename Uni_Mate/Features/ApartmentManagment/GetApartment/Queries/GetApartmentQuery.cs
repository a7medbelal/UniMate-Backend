
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


        /**
         * TO DO:
         * add favourite in the future once implemented
         * Done :)
         */
 
         var userId = _userInfo.ID; 
        var favouriteCommand = new GetApartmentFavoriteQuery(userId);//_userInfo.ID);
        var result = await _mediator.Send(favouriteCommand, cancellationToken);
        if (!result.isSuccess)
        {
            return RequestResult<Pagination<GetApartmentDTO>>.Failure(ErrorCode.NotFound, "Favourites not found");
        }
        var favourites = result.data;
        var query = _repository.GetAll()
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
                Favourite = favourites.Contains(x.Id)
            }).OrderBy(x => x.Id);

        // var query = _repository.GetAll()
        //.Select(x => new GetApartmentDTO
        //{
        //    Id = x.Id,
        //    Images = (List<string>)x.Images.Select(i => i.ImageUrl),
        //    Location = x.Location,
        //    Gender = x.Gender,
        //    Floor = x.Floor ?? "unknown",
        //    DetailedAddress = x.DescripeLocation ?? "unknown",
        //    OwnerName = (x.Owner != null ? x.Owner.Fname + " " + x.Owner.Lname : string.Empty),
        //    NumberOfRooms = x.Rooms != null ? x.Rooms.Count() : 0,
        //    Price = x.Rooms != null && x.Rooms.Any() ? x.Rooms.FirstOrDefault().Price : 0,
        //    Favourite = favourites.Any(y => y == x.Id)
        //});
        //await query.ToListAsync();
        //Console.WriteLine($"Cold: {stopwatch.ElapsedMilliseconds} ms");

        //// Warm
        //stopwatch.Restart();
        //await query.ToListAsync();
        //stopwatch.Stop();




        var paginatedResult = await Pagination<GetApartmentDTO>.ToPagedList(query, request.PageNumber, request.PageSize);

        var apartmentIds = paginatedResult.Select(a => a.Id).ToList();
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var imageTask = await _imageRepository.Get(i => apartmentIds.Contains(i.ApartmentId))
            .Select(i => new { i.ApartmentId, i.ImageUrl })
            .ToListAsync();

        Console.WriteLine($"Second Query : {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Stop();
            

        var imagesGrouped = imageTask
            .GroupBy(i => i.ApartmentId)
            .ToDictionary(g => g.Key, g => g.Select(i => i.ImageUrl).ToList());

        foreach (var apartment in paginatedResult)
        {
            apartment.Images = imagesGrouped.TryGetValue(apartment.Id, out var imgs)
                ? imgs
                : new List<string>();
        }



        return RequestResult<Pagination<GetApartmentDTO>>.Success(paginatedResult, "Pagination Worked");
    }


}




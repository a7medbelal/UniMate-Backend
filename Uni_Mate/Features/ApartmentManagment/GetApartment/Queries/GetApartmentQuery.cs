using MediatR;
using System.Linq;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
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








        /**
         * TO DO:
         * add favourite in the future once implemented
         * Done :)
         */
 
         var userId = _userInfo.ID; 
        var favouriteCommand = new GetApartmentFavoriteQuery(userId);//_userInfo.ID);
        var result = _mediator.Send(favouriteCommand, cancellationToken);
        if (!result.IsCompletedSuccessfully)
        {
            return RequestResult<Pagination<GetApartmentDTO>>.Failure(ErrorCode.NotFound, "Favourites not found");
        }
        var favourites = result.Result.data;
        var query = _repository.GetAll()
            .Select(x => new GetApartmentDTO
            {
              
                Images = (List<string>)x.Images.Select(i => i.ImageUrl),
                Address = x.Location,
                Gender = x.Gender.ToString(),
                Floor = x.Floor ?? "unknown",
                OwnerName = (x.Owner != null ? x.Owner.Fname + " " + x.Owner.Lname : string.Empty),
                NumberOfRooms = x.Rooms != null ? x.Rooms.Count() : 0,
                Facilities = x.ApartmentFacilities.Select(f => f.Facility.Name).ToList(),
                Price = x.Rooms != null && x.Rooms.Any() ? x.Rooms.FirstOrDefault().Price : 0,
                Favourite = favourites.Any(y => y == x.Id)
            });

        var paginatedResult = await Pagination<GetApartmentDTO>.ToPagedList(query, request.PageNumber, request.PageSize);

        return RequestResult<Pagination<GetApartmentDTO>>.Success(paginatedResult, "Pagination Worked");
    }
}
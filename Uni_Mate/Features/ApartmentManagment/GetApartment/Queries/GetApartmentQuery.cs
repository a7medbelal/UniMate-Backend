using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.GetApartment.Queries;
public record GetApartmentQuery(int PageNumber, int PageSize) : IRequest<RequestResult<Pagination<GetApartmentDTO>>>;

public class GetApartmentQueryHandler : BaseRequestHandler<GetApartmentQuery, RequestResult<Pagination<GetApartmentDTO>>, Apartment>
{
    public GetApartmentQueryHandler(BaseRequestHandlerParameter<Apartment> parameters) : base(parameters)
    {
    }
    public override async Task<RequestResult<Pagination<GetApartmentDTO>>> Handle(GetApartmentQuery request, CancellationToken cancellationToken)
    {
        var test = _repository.GetAll().ToList();
        var query = _repository.GetAll()
            .Select(x => new GetApartmentDTO
             {
                 Address = x.Location,
                 Gender = x.Gender.ToString(),
                 Floor = x.Floor,
                 OwnerName = (x.Owner != null ? x.Owner.Fname + " " + x.Owner.Lname : string.Empty),
                 NumberOfRooms = x.Rooms != null ? x.Rooms.Count() : 0,
                 Price = x.Rooms != null && x.Rooms.Any() ? x.Rooms.FirstOrDefault().Price : 0
             });
        var paginatedResult = await Pagination<GetApartmentDTO>.ToPagedList(query, request.PageNumber, request.PageSize);

        return RequestResult<Pagination<GetApartmentDTO>>.Success(paginatedResult, "Pagination Worked");
    }
}

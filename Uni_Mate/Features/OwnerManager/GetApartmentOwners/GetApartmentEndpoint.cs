using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Org.BouncyCastle.Bcpg.Sig;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.GetApartment.Queries;
using Uni_Mate.Features.Common.ApartmentManagement.ApartmerntDTO;
using Uni_Mate.Features.OwnerManager.GetApartmentOwners.Queries;
using Uni_Mate.Filters;
using Uni_Mate.Models.UserManagment.Enum;

namespace Uni_Mate.Features.ApartmentManagment.GetApartment;

public class GetApartmentOwnerEndpoint : BaseWithoutTRequestEndpoint<List<GetApartmentOwnerResponseViewModel>>
{
    public GetApartmentOwnerEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
    {
    }
    [Authorize]
    [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] {Feature.GetApartmentsOwner   })]
    [HttpGet]
    public async Task<EndpointResponse<List<GetApartmentOwnerResponseViewModel>>> GetApartmentsOwner()
    {
        var apartments = await _mediator.Send(new GetApartmentsOwnerQuery());
        if (!apartments.isSuccess)
            return EndpointResponse<List<GetApartmentOwnerResponseViewModel>>.Failure(apartments.errorCode, apartments.message);

        var response =
              apartments.data.Select(x => new GetApartmentOwnerResponseViewModel
              {
                  Id = x.Id,
                  Images = x.Images,
                  DetailedAddress = x.DetailedAddress,
                  Location = x.Location,
                  Gender = x.Gender,
                  Floor = x.Floor,
                  OwnerName = x.OwnerName,
                  NumberOfRooms = x.NumberOfRooms,
                  Price = x.Price,

              }).ToList();
        return EndpointResponse<List<GetApartmentOwnerResponseViewModel>>.Success(response, "Got Apartments Successfully");
    }
}

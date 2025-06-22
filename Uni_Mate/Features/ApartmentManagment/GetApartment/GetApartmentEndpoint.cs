using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.GetApartment.Queries;

namespace Uni_Mate.Features.ApartmentManagment.GetApartment;

public class GetApartmentEndpoint : BaseEndpoint<GetApartmentViewModel, GetApartmentResponseViewModel>
{
    public GetApartmentEndpoint(BaseEndpointParameters<GetApartmentViewModel> parameters) : base(parameters)
    {
    }
    [HttpGet]
    public async Task<EndpointResponse<GetApartmentResponseViewModel>> GetApartment([FromQuery] GetApartmentViewModel viewmodel)
    {
        var validationResult = ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
            return validationResult;

        var apartments = await _mediator.Send(new GetApartmentQuery(viewmodel.PageNumber, viewmodel.PageSize));
        if (!apartments.isSuccess)
            return EndpointResponse<GetApartmentResponseViewModel>.Failure(apartments.errorCode, apartments.message);
        
        var response = new GetApartmentResponseViewModel
        {
            Apartments = apartments.data.Select(x => new GetApartmentDTO
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
                Favourite = x.Favourite
            }).ToList(),
            TotalCount = apartments.data.TotalCount,
            PageNumber = apartments.data.CurrentPage,
            PageSize = apartments.data.pageSize,
            TotalPages = apartments.data.TotalPages,
        };
        return EndpointResponse<GetApartmentResponseViewModel>.Success(response, "Got Apartments Successfully");
    }
}

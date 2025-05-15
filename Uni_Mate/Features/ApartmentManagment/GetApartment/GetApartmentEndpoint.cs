using Microsoft.AspNetCore.Mvc;
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
        {
            return EndpointResponse<GetApartmentResponseViewModel>.Failure(ErrorCode.ApartmnetFetchFailed, "");
        }

        var apartments = await _mediator.Send(new GetApartmentQuery(viewmodel.PageNumber, viewmodel.PageSize));
        if (!apartments.isSuccess)
        {
            return EndpointResponse<GetApartmentResponseViewModel>.Failure(ErrorCode.ApartmnetFetchFailed, "");
        }
        var response = new GetApartmentResponseViewModel
        {
            Apartments = apartments.data.Select(x => new GetApartmentDTO
            {
                Address = x.Address,
                Gender = x.Gender,
                Floor = x.Floor,
                OwnerName = x.OwnerName,
                NumberOfRooms = x.NumberOfRooms,
                Price = x.Price
            }).ToList(),
            TotalPages = apartments.data.TotalPages,
            PageSize = apartments.data.pageSize,
            CurrentPage = apartments.data.CurrentPage,
        };
        return EndpointResponse<GetApartmentResponseViewModel>.Success(response, "Got Apartments Successfully");
    }
}

using Azure;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.SearchForApartment.Queries;

namespace Uni_Mate.Features.ApartmentManagment.SearchForApartment
{
    public class SearchForApartmentEndpoint : BaseEndpoint<ApartmentParames, ResponseViewModelForFilter>
    {
        public SearchForApartmentEndpoint(BaseEndpointParameters<ApartmentParames> parameters) : base(parameters)
        {
        }

        [HttpGet]

        public async Task<EndpointResponse<Pagination<ResponseViewModelForFilter>>> SearchForApartment([FromQuery] ApartmentParames parames)
        {
            var validationResult = ValidateRequest(parames);
            if (!validationResult.isSuccess)
                return EndpointResponse< Pagination<ResponseViewModelForFilter>>.Failure(validationResult.errorCode,validationResult.message);  

            // Call the mediator to handle the request
            // exct 

            var result = await _mediator.Send(new SearchForApartmentQurey(parames.Keyword ,parames.FromPrice,parames.ToPrice,parames.Capacity , parames.Location, parames.Gender,parames.PageSize ,parames.PageNumber ,parames.SortBy));
            if (!result.isSuccess) 
               return EndpointResponse<Pagination<ResponseViewModelForFilter>>.Failure(result.errorCode, result.message);

            var response = result.data.Adapt<Pagination<ResponseViewModelForFilter>>();

            return EndpointResponse<Pagination<ResponseViewModelForFilter>>.Success(response, result.message);
        }
        
    }
}

using Azure;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.GetApartment;
using Uni_Mate.Features.ApartmentManagment.SearchForApartment.Queries;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.SearchForApartment
{
    public class SearchForApartmentEndpoint : BaseEndpoint<ApartmentParames, ResponseViewModelForFilter>
    {
        public SearchForApartmentEndpoint(BaseEndpointParameters<ApartmentParames> parameters) : base(parameters)
        {
        }

        [HttpGet]

        public async Task<EndpointResponse<ResponseViewModelForFilter>> SearchForApartment([FromQuery] ApartmentParames parames)
        {
            var validationResult = ValidateRequest(parames);
            if (!validationResult.isSuccess)
                return EndpointResponse<ResponseViewModelForFilter>.Failure(validationResult.errorCode,validationResult.message);  

            // Call the mediator to handle the request
            // exct 

            var result = await _mediator.Send(new SearchForApartmentQurey(parames.Keyword ,parames.FromPrice,parames.ToPrice,parames.Capacity , parames.Location, parames.Gender,parames.PageSize ,parames.PageNumber ,parames.SortBy));
            if (!result.isSuccess) 
               return EndpointResponse<ResponseViewModelForFilter>.Failure(result.errorCode, result.message);
            // Map the result to the response view model
            var response = new ResponseViewModelForFilter
            {
                DTOs = result.data.Select(x => new GetAparmtmentFilterDTO
                {
                    Images = x.Images,
                    Location = x.Location,
                    Capecity = x.Capecity,
                    Gender = x.Gender,
                    Floor = x.Floor,
                    OwnerName = x.OwnerName,
                    NumberOfRooms = x.NumberOfRooms,
                    Price = x.Price
                }).ToList(),
                TotalCount = result.data.TotalCount,
                PageNumber = parames.PageNumber,
                PageSize = parames.PageSize,
                TotalPages = result.data.TotalPages,
            };

            return EndpointResponse<ResponseViewModelForFilter>.Success(response, result.message);
        }
        
    }
}

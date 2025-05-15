using Uni_Mate.Common.BaseEndpoints;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.ApartmentDTO;
using Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails.Quarry;
namespace Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails
{
    public class ApartmentDetailsEndpoint : BaseEndpoint<ApartmentDetailsVM, ApartmentDetailsDTO>
    {
        public ApartmentDetailsEndpoint(BaseEndpointParameters<ApartmentDetailsVM> parameters) : base(parameters)
        { }

        [HttpGet]
        public async Task<EndpointResponse<ApartmentDetailsDTO>> ApartmentDetails([FromBody]ApartmentDetailsVM request)
        {
            var result = ValidateRequest(request);
            if(result.isSuccess == false)
            {
                return EndpointResponse<ApartmentDetailsDTO>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, result.message);
            }

            var res =await  _mediator.Send(new ApartmentDetailsQuarry(request.id));
            if(res.isSuccess)
            {
                return EndpointResponse<ApartmentDetailsDTO>.Success(res.data, "Apartment Details Retrieved Successfully");
            }
            return EndpointResponse<ApartmentDetailsDTO>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound,res.message);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Features.SeedingData
{
    public class SeedingDataEndpoint : BaseWithoutTRequestEndpoint<bool>
    {
        public SeedingDataEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
        {
        }
        [HttpGet]
        public async Task<EndpointResponse<bool>> SeedData()
        {
            try
            {
                // Call the seeding service to seed data
                var result = await _mediator.Send(new SeedingDataCommand());
                if (!result.isSuccess)
                {
                    return EndpointResponse<bool>.Failure(result.errorCode, result.message);
                }
                return EndpointResponse<bool>.Success(result.data);
            }
            catch (Exception ex)
            {
                return EndpointResponse<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InternalServerError, ex.Message);
            }
        }
    }
}

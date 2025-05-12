using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CreateFullApartmentOrcasterartor;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess
{
    public class SubmitPostEndPoint : BaseEndpoint<SubmitPostViewModel, bool>
    {
        public SubmitPostEndPoint(BaseEndpointParameters<SubmitPostViewModel> parameters) : base(parameters)
        {
        }
        [HttpPost]
        public async Task<EndpointResponse<bool>> SubmitPost([FromForm] SubmitPostViewModel viewmodel, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new SubmitPostCommand(
                viewmodel.Num,
                viewmodel.Location,
                viewmodel.Description,
                viewmodel.DescribeLocation,
                viewmodel.Floor,
                viewmodel.Capecity,
                viewmodel.NumberOfRooms,
                viewmodel.GenderAcceptance,
                viewmodel.DurationType,
                viewmodel.Rooms,
                viewmodel.CategoryFacilities
            ));
            if (!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }
            return EndpointResponse<bool>.Success(result.data, result.message);
        }
    }
}

using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Authoraztion.RegisterUser.Commands;

namespace Uni_Mate.Features.Authoraztion.RegisterUser.RegisterStudent
{
    public class RegisterStudentEndPoint : BaseEndpoint<RegisterStudentRequestViewModel, bool>
    {
        public RegisterStudentEndPoint(BaseEndpointParameters<RegisterStudentRequestViewModel> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> RegisterStudent([FromBody] RegisterStudentRequestViewModel request)
        {
            // Validate the inputs 
            var validationResponse = ValidateRequest(request);
            if (!validationResponse.isSuccess)
                return validationResponse;

            var command = new RegisterUserCommand(request.Fname, request.Lname, request.UserName, request.Email, request.Password, request.NationalId);

            var result = await _mediator.Send(command);
            if (!result.isSuccess)
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);

            return EndpointResponse<bool>.Success(result.data, result.message);

        }

    }
}
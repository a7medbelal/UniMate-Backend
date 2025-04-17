using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Authoraztion.RegisterUser.Commands;

namespace Uni_Mate.Features.Authoraztion.RegisterUser
{
    public class RegisterStudentEndPoint : BaseEndpoint<RegisterUserRequestViewModel, bool>
    {
        public RegisterStudentEndPoint(BaseEndpointParameters<RegisterUserRequestViewModel> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>>  RegisterStudent([FromBody] RegisterUserRequestViewModel request)
        {
            // Validate the inputs 
            var validationResponse = ValidateRequest(request);
            if (!validationResponse.isSuccess)
                   return validationResponse;
           


            var command = new RegisterUserCommand(request.UserName,request.Email, request.Password, request.Name, request.PhoneNo, request.Country);
            var result = await _mediator.Send(command);
            if (result.isSuccess)
                return EndpointResponse<bool>.Failure(result.errorCode ,result.message); 
            
            return EndpointResponse<bool>.Success(result.data, result.message);

        }

    }





}

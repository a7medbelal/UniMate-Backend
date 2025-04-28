using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Authoraztion.RegisterUser.Commands;

namespace Uni_Mate.Features.Authoraztion.RegisterUser.RegisterOwner
{
	public class RegisterOwnerEndPoint : BaseEndpoint<RegisterOwnerRequestViewModel, bool>
	{
		public RegisterOwnerEndPoint(BaseEndpointParameters<RegisterOwnerRequestViewModel> parameters) : base(parameters)
		{
		}

		[HttpPost]
		public async Task<EndpointResponse<bool>> RegisterOwner([FromBody] RegisterOwnerRequestViewModel request)
		{
			// Validate the inputs 
			var validationResponse = ValidateRequest(request);
			if (!validationResponse.isSuccess)
				return validationResponse;

			// The national ID is not required for the owner so we can pass null, and same for the username which is labeled as optional in the request
			var command = new RegisterOwnerCommand(request.Email, request.Password, request.FName, request.LName, request.PhoneNo, "", "Owner", null);
			var result = await _mediator.Send(command);
			if (!result.isSuccess)
				return EndpointResponse<bool>.Failure(result.errorCode, result.message);

			return EndpointResponse<bool>.Success(result.data, result.message);
		}
	}
}

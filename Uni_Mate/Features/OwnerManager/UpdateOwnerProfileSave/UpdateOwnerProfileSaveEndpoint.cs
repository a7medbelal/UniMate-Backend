using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.OwnerManager.UpdateOwnerProfileSave.Command;

namespace Uni_Mate.Features.OwnerManager.UpdateOwnerProfileSave
{
	[Authorize]
	public class UpdateOwnerProfileSaveEndpoint : BaseEndpoint<UpdateOwnerProfileSaveViewModel, bool>
	{
		public UpdateOwnerProfileSaveEndpoint(BaseEndpointParameters<UpdateOwnerProfileSaveViewModel> parameters) : base(parameters)
		{
		}

		[HttpPost]
		public async Task<EndpointResponse<bool>> UpdateProfileSave([FromBody] UpdateOwnerProfileSaveViewModel request)
		{
			var result = _validator.Validate(request);
			if (result.IsValid)
			{
				var response = await _mediator.Send(new UpdateOwnerProfileSaveCommand(request.Fname, request.Lname, request.Email, request.Phones, request.BriefOverView));

				return EndpointResponse<bool>.Success(response.data, response.message);
			}
			else
			{
				var validationError = string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
				return EndpointResponse<bool>.Failure(ErrorCode.InvalidData, validationError);
			}
		}
	}
}
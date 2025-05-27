using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.UploadImageCommand;
using Uni_Mate.Features.StudentManager.LoginInfoSave.Command;

namespace Uni_Mate.Features.StudentManager.LoginInfoSave
{
    [Authorize]
    public class LoginInfoSaveEndpoint : BaseEndpoint<LoginInfoSaveVM,bool>
    {
        public LoginInfoSaveEndpoint(BaseEndpointParameters<LoginInfoSaveVM> parameters):base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> LoginInfoSave([FromForm] LoginInfoSaveVM loginInfoSaveVM)
        {
            var result = ValidateRequest(loginInfoSaveVM);

            if(result.isSuccess == false)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }

            string frontUrl = string.Empty;
            if (loginInfoSaveVM.FrontImage != null)
            {
                var frontImage =await _mediator.Send(new UploadImageCommand(loginInfoSaveVM.FrontImage));
                if (frontImage.isSuccess == false)
                {
                    return EndpointResponse<bool>.Failure(frontImage.errorCode, frontImage.message);
                }
                frontUrl = frontImage.data;
            }

            string backUrl = string.Empty;
            if (loginInfoSaveVM.BackImage != null)
            {
                var backImage = await _mediator.Send(new UploadImageCommand(loginInfoSaveVM.BackImage));
                if (backImage.isSuccess == false)
                {
                    return EndpointResponse<bool>.Failure(backImage.errorCode, backImage.message);
                }
                backUrl = backImage.data;
            }

            var resultSave = await _mediator.Send(new LoginInfoSaveCommand(
                loginInfoSaveVM.Email,
                loginInfoSaveVM.National_Id,
                frontUrl,
                backUrl
            ));

            if (resultSave.isSuccess == false)
            {
                return EndpointResponse<bool>.Failure(resultSave.errorCode, resultSave.message);
            }

            return EndpointResponse<bool>.Success(resultSave.data, resultSave.message);
        }

    }
}

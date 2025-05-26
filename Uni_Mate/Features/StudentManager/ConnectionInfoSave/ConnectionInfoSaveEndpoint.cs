using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.StudentManager.ConnectionInfoSave.Command;

namespace Uni_Mate.Features.StudentManager.ConnectionInfoSave
{
    [Authorize]
    public class ConnectionInfoSaveEndpoint : BaseEndpoint<ConnectionInfoVM,bool>
    {
        public ConnectionInfoSaveEndpoint(BaseEndpointParameters<ConnectionInfoVM> parameters) : base(parameters)
        {
        }
        [HttpPost]
        public async Task<EndpointResponse<bool>> ConnectionSave([FromBody]ConnectionInfoVM request)
        {
            var validate = ValidateRequest(request);
            if(!validate.isSuccess)
            {
                return EndpointResponse<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, validate.message);
            }

            var result = await _mediator.Send(new ConnectionInfoCommand(request.PhoneNum,request.AnotherPhoneNum,request.WhatAppLink,request.FaceBookLink));

            if(!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }

            return EndpointResponse<bool>.Success(true, result.message);
        }
    }

}

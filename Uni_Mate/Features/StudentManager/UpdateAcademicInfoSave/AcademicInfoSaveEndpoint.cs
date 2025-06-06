﻿using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.UploadImageCommand;
using Uni_Mate.Features.StudentManager.UpdateAcademicInfoSave.Command;

namespace Uni_Mate.Features.StudentManager.UpdateAcademicInfoSave
{
    public class AcademicInfoSaveEndpoint : BaseEndpoint<AcademicInfoVM, bool>
    {
        public AcademicInfoSaveEndpoint(BaseEndpointParameters<AcademicInfoVM> parameters) : base(parameters)
        {
        }
        [HttpPost]
        public async Task<EndpointResponse<bool>> UpdateAcademicInfo([FromBody] AcademicInfoVM command)
        {
            //var linkImage = await _mediator.Send(new UploadImageCommand(command.KarnihImage));
            var result = await _mediator.Send(new AcademicInfoSaveCommand(
                command.University,
                command.Faculty,
                command.AcademicYear,
                command.Department,
                command.KarnihImage
                //linkImage.data
            ));


            if (!result.isSuccess)
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);

            return EndpointResponse<bool>.Success(result.data, result.message);
        }
    }
}

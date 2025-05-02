using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrelloCopy.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.StudentManager.GetStudent.Quarry;

namespace Uni_Mate.Features.StudentManager.GetStudent
{
    [Authorize]
    public class GetStudentEndpoint : BaseWithoutTRequestEndpoint<GetStudentEndpoint>
    {
        public GetStudentEndpoint(BaseWithoutTRequestEndpointParameters parameters) : base(parameters)
        {
        }

        [HttpGet]
        public EndpointResponse<GetStudentDTO> GetStudent()
        {
            var studentResult = _mediator.Send(new GetStudentQuarry()).Result;
            if(!studentResult.isSuccess)
            {
                return EndpointResponse<GetStudentDTO>.Failure(ErrorCode.NotFound, "Student not found");
            }
            return EndpointResponse<GetStudentDTO>.Success(studentResult.data, "Student retrieved successfully");
        }
    }
}

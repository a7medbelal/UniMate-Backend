using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.CommentManagement.AddComment.Command;

namespace Uni_Mate.Features.CommentManagement.AddComment
{
    [Authorize]
    public class AddCommentEndpoint : BaseEndpoint<AddCommentVM, bool>
    {
        public AddCommentEndpoint(BaseEndpointParameters<AddCommentVM> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> AddComment([FromBody]AddCommentVM request)
        {
            var check = ValidateRequest(request);
            if (!check.isSuccess)
            {
                return EndpointResponse<bool>.Failure(check.errorCode, check.message);
            }
            var command = new AddCommentCommand(request.Message, request.ApartmentId);
            var result = await _mediator.Send(command);
            if (!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }
            return EndpointResponse<bool>.Success(result.data, result.message);
        }
    }
}

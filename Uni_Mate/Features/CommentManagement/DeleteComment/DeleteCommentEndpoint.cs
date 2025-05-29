using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.CommentManagement.DeleteComment.Command;

namespace Uni_Mate.Features.CommentManagement.DeleteComment
{
    [Authorize]
    public class DeleteCommentEndpoint : BaseEndpoint<DeleteCommentVM, bool>
    {
        public DeleteCommentEndpoint(BaseEndpointParameters<DeleteCommentVM> parameters) : base(parameters)
        {
        }
        [HttpDelete]
        public async Task<EndpointResponse<bool>> DeleteComment([FromHeader] DeleteCommentVM request)
        {
            var check = ValidateRequest(request);
            if (!check.isSuccess)
            {
                return EndpointResponse<bool>.Failure(check.errorCode, check.message);
            }
            var command = new DeleteCommentCommand(request.CommentId,request.ApartmentId);
            var result = await _mediator.Send(command);
            if (!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }
            return EndpointResponse<bool>.Success(result.data, result.message);
        }
    }
 
}

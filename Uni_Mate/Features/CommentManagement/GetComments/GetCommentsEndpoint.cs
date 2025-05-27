using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.CommentManagement.GetComments.Commands;

namespace Uni_Mate.Features.CommentManagement.GetComments;
public class GetCommentEndpoint : BaseEndpoint<GetCommentsViewModel, List<GetCommentsDTO>>
{
    public GetCommentEndpoint(BaseEndpointParameters<GetCommentsViewModel> parameters) : base(parameters)
    {
    }
    [HttpGet]
    public async Task<EndpointResponse<List<GetCommentsDTO>>> GetComments([FromQuery] GetCommentsViewModel viewmodel)
    {
        var validationResult = ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
        {
            return validationResult;
        }
        var comments = await _mediator.Send(new GetCommentsCommand(viewmodel.ApartmentId));
        if (!comments.isSuccess)
        {
            return EndpointResponse<List<GetCommentsDTO>>.Failure(ErrorCode.NotFound, "no comments found");
        }
        return EndpointResponse<List<GetCommentsDTO>>.Success(comments.data, "Comments retrieved successfully.");
    }
}

using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.Comment_Review;

namespace Uni_Mate.Features.CommentManagement.DeleteComment.Command
{
    public record DeleteCommentCommand(int CommentId, int ApartmentId): IRequest<RequestResult<bool>>;
    public class DeleteCommentHandler:BaseRequestHandler<DeleteCommentCommand, RequestResult<bool>, Comment>
    {
        public DeleteCommentHandler(BaseRequestHandlerParameter<Comment> parameters) : base(parameters)
        {
        }
        public override async Task<RequestResult<bool>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userInfo.ID;
            if (string.IsNullOrEmpty(userId) || userId == "-1")
            {
                return RequestResult<bool>.Failure(ErrorCode.Unauthorized, "User not authorized.");
            }

            if (request.CommentId <= 0 || request.ApartmentId <= 0)
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidData, "Invalid comment or apartment ID.");
            }
            var comment = await _repository.GetByIDAsync(request.CommentId);
            if (comment == null || comment.ApartmentId != request.ApartmentId || comment.StudentId != userId)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Comment not found or does not belong to the specified apartment Or To The User.");
            }
            await _repository.HardDelete(comment);
            await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Comment deleted successfully.");
        }
    }
}

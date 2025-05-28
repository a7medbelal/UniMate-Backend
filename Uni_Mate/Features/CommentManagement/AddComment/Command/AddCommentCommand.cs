using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.Comment_Review;

namespace Uni_Mate.Features.CommentManagement.AddComment.Command
{
    public record AddCommentCommand(string? Message, int ApartmentId):IRequest<RequestResult<bool>>;

    public class AddCommentHandler:BaseRequestHandler<AddCommentCommand, RequestResult<bool>,Comment>
    {
        private readonly IRepository<Apartment> _apartRepo; 
        public AddCommentHandler(BaseRequestHandlerParameter<Comment> parameters,
            IRepository<Apartment> apartRepo) : base(parameters)
        {
            _apartRepo = apartRepo;
        }

        public override async Task<RequestResult<bool>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userInfo.ID;
            if (string.IsNullOrEmpty(request.Message) || userId == "-1")
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidData, "Invalid input data.");
            }

            if (request.ApartmentId <= 0)
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidData, "Invalid apartment ID.");
            }

            var apartment =await _apartRepo.GetByIDAsync(request.ApartmentId);
            if (apartment == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Apartment not found.");
            }
            var comment = new Comment
            {
                Message = request.Message,
                ApartmentId = request.ApartmentId,
                StudentId = userId,
                CreatedDate = DateTime.UtcNow
            };

            await _repository.AddAsync(comment);
            await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Comment added successfully.");
        }
    }
}

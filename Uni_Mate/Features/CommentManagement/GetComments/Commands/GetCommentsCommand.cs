using MediatR;
using System.Data.Entity;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.Comment_Review;

namespace Uni_Mate.Features.CommentManagement.GetComments.Commands;

public record GetCommentsCommand(int ApartmentId) : IRequest<RequestResult<List<GetCommentsDTO>>>;

public class GetCommentsCommandHandler : BaseRequestHandler<GetCommentsCommand, RequestResult<List<GetCommentsDTO>>, Comment>
{
    public GetCommentsCommandHandler(BaseRequestHandlerParameter<Comment> parameters) : base(parameters)
    {
    }
    public async override Task<RequestResult<List<GetCommentsDTO>>> Handle(GetCommentsCommand request, CancellationToken cancellationToken)
    {
        var comments = await _repository.GetAll()
            .Where(c => c.ApartmentId == request.ApartmentId)
            .Select(x => new GetCommentsDTO
            {
                Message = x.Message,
                StudentName = x.Student.Fname + " " + x.Student.Lname,
                StudentImage = x.Student.Image,
                CreatedDate = x.CreatedDate
            }).ToListAsync();
        return RequestResult<List<GetCommentsDTO>>.Success(comments, "Comments retrieved successfully.");
    }
}

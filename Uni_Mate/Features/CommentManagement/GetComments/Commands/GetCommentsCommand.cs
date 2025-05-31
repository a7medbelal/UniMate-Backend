using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
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

        if (request.ApartmentId <= 0)
            return RequestResult<List<GetCommentsDTO>>.Failure(ErrorCode.InvalidData, "Invalid apartment ID");

        var comments = await _repository
        .Get(c => c.ApartmentId == request.ApartmentId)
        .Select(c => new GetCommentsDTO
         {
        Id = c.Id,
          Message = c.Message ?? "",
           StudentName = string.IsNullOrEmpty(c.Student!.Lname)
            ? c.Student.Fname ?? "Unknown"
            : $"{c.Student.Fname} {c.Student.Lname}",
        StudentImage = c.Student.Image ?? "default-profile.png",
        CreatedDate = c.CreatedDate
    })
    .ToListAsync(cancellationToken);
        return RequestResult<List<GetCommentsDTO>>.Success(comments, "تم جلب التعليقات بنجاح"); 
    }

}

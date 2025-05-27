using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.StudentManager.LoginInfoDisplay.Quarry
{
    public record LoginInfoDisplayQuarry(): IRequest<RequestResult<LoginInfoDisplayDTO>>;

    public class LoginInfoDisplayQuarryHandler : BaseWithoutRepositoryRequestHandler<LoginInfoDisplayQuarry, RequestResult<LoginInfoDisplayDTO>, Student>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginInfoDisplayQuarryHandler(BaseWithoutRepositoryRequestHandlerParameters<Student>parameters,IHttpContextAccessor httpContextAccessor) : base(parameters)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<RequestResult<LoginInfoDisplayDTO>> Handle(LoginInfoDisplayQuarry request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RequestResult<LoginInfoDisplayDTO>.Failure(ErrorCode.NotFound, "User not found");
            }

            Student student =await  _repositoryIdentity.GetByIDAsync(userId);
            if (student == null)
            {
                return RequestResult<LoginInfoDisplayDTO>.Failure(ErrorCode.NotFound, "Student not found");
            }

            var studentDto = new LoginInfoDisplayDTO(student.Email,student.National_Id,student.FrontPersonalImage,student.BackPersonalImage);
            return RequestResult<LoginInfoDisplayDTO>.Success(studentDto, "Student retrieved successfully");
        }
    }
}

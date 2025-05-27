using Mapster;
using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Mapping;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Features.StudentManager.UpdateProfileDisplay.Quarry
{
    public record UpdateProfileDisplayQuarry() : IRequest<RequestResult<UpdateProfileDisplayDTO>>;

    public class UpdateProfileDisplayQuarryHandler : BaseWithoutRepositoryRequestHandler<UpdateProfileDisplayQuarry, RequestResult<UpdateProfileDisplayDTO>, User>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateProfileDisplayQuarryHandler(BaseWithoutRepositoryRequestHandlerParameters<User> parameters, IHttpContextAccessor httpContextAccessor) : base(parameters)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<RequestResult<UpdateProfileDisplayDTO>> Handle(UpdateProfileDisplayQuarry request, CancellationToken cancellation)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RequestResult<UpdateProfileDisplayDTO>.Failure(ErrorCode.NotFound, "User not found");
            }
            var student = await _repositoryIdentity.GetByIDAsync(userId);
            if (student == null)
            {
                return RequestResult<UpdateProfileDisplayDTO>.Failure(ErrorCode.NotFound, "Student not found");
            }
            if (student == null)
            {
                return RequestResult<UpdateProfileDisplayDTO>.Failure(ErrorCode.NotFound, "Student not found");
            }
            var studentDto = student.Adapt<UpdateProfileDisplayDTO>(MapsterConfig.Configure());
            return RequestResult<UpdateProfileDisplayDTO>.Success(studentDto, "Student retrieved successfully");
        }
    }
}

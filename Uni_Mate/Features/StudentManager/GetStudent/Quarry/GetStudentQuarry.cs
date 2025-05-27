using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;
using Mapster;
using Uni_Mate.Common.Mapping;
using System.Security.Claims;

namespace Uni_Mate.Features.StudentManager.GetStudent.Quarry
{
    public record GetStudentQuarry() : IRequest<RequestResult<GetStudentDTO>>;

    public class GetStudentQuarryHandler : BaseWithoutRepositoryRequestHandler<GetStudentQuarry, RequestResult<GetStudentDTO>, Student>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetStudentQuarryHandler(BaseWithoutRepositoryRequestHandlerParameters<Student> parameters, IHttpContextAccessor httpContextAccessor) : base(parameters)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<RequestResult<GetStudentDTO>> Handle(GetStudentQuarry request, CancellationToken cancellationToken)
        {

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RequestResult<GetStudentDTO>.Failure(ErrorCode.NotFound, "User not found");
            }
            Student student;
            try
            {
                student = _repositoryIdentity.GetInclude(st => st.Id == userId, new string[] { "Phones" });
                if (student == null)
                {
                    return RequestResult<GetStudentDTO>.Failure(ErrorCode.NotFound, "Student not found");
                }
            }
            catch (Exception ex)
            {
                return RequestResult<GetStudentDTO>.Failure(ErrorCode.InternalServerError, $"An error occurred while retrieving the student: {ex.Message}");
            }
            if (student == null)
            {
                return RequestResult<GetStudentDTO>.Failure(ErrorCode.NotFound, "Student not found");
            }
            var studentDto = student.Adapt<GetStudentDTO>(MapsterConfig.Configure());
            return RequestResult<GetStudentDTO>.Success(studentDto, "Student retrieved successfully");
        }
    }

}

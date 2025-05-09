using MediatR;
using System.Security.Claims;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.UserManagment;


namespace Uni_Mate.Features.StudentManager.UpdateAcademicInfoSave.Command
{
    public record AcademicInfoSaveCommand(string? University, string? Faculty, string? AcademicYear, string? Department, string? KarnihImage) : IRequest<RequestResult<bool>>;

    public class AcademicInfoSaveHandler : BaseWithoutRepositoryRequestHandler<AcademicInfoSaveCommand, RequestResult<bool>, Student>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public  AcademicInfoSaveHandler(BaseWithoutRepositoryRequestHandlerParameters<Student> parameters,IHttpContextAccessor httpContextAccessor) : base(parameters)
        {
            _httpContextAccessor =httpContextAccessor;
        }
        public override async Task<RequestResult<bool>> Handle(AcademicInfoSaveCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "User Not Found");

            var student = await _repositoryIdentity.GetByIDAsync(userId);
            if (student == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Student not found");

            student.University = request.University;
            student.Faculty = request.Faculty;
            student.AcademicYear = request.AcademicYear;
            student.Department = request.Department;
            student.KarnihImage = request.KarnihImage;

            await _repositoryIdentity.UpdateAsync(student);


            return RequestResult<bool>.Success(true, $"Academic Info Updated and this is the link {request.KarnihImage}");
        }
    }

        
    

}
